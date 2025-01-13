using System.Collections.Concurrent;
using System.Diagnostics;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Translate.Data;
using Translate.Models;

namespace Translate.Services;

public class CollectService : ICollectService
{
    private readonly CollectorOptions _collectorOptions;
    private readonly TranslateDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    private ConcurrentBag<long> _bag;

    public CollectService(IOptions<CollectorOptions> collectorOptions, TranslateDbContext context, IServiceProvider serviceProvider)
    {
        _collectorOptions = collectorOptions.Value;
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task CollectFromFolderAsync(string folder, bool sink = false)
    {
        var dir = new DirectoryInfo(folder);
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException();
        }

        var files = dir.EnumerateFiles(searchPattern: "*", searchOption: SearchOption.AllDirectories);
        foreach (var file in files)
        {
            await CollectFragment(file, sink);
        }
    }

    public async Task CollectFormFileAsync(string path, bool sink = false)
    {
        var file = new FileInfo(path);
        if (!file.Exists)
        {
            throw new FileNotFoundException();
        }

        await CollectFragment(file, sink);
    }

    protected async Task CollectFragment(FileInfo file, bool sink = false)
    {
        var collectorTypes = _collectorOptions.FragmentCollectors;

        foreach (var collectorType in collectorTypes)
        {
            var collector = _serviceProvider.GetRequiredService(collectorType) as IFragmentCollector;
            Debug.Assert(collector != null, nameof(collector) + " != null");
            if (collector.Satisfied(file))
            {
                var context = await collector.CollectAsync(file);
                if (sink)
                {
                    await SinkAsync(context);
                }
                return;
            }
        }
    }

    public async Task SinkAsync(CollectContext context)
    {
        if (_bag == null)
        {
            _bag = new ConcurrentBag<long>();

            var list = await _context.TranslateTexts.Select(a => a.Id).ToListAsync();
            foreach (var item in list)
            {
                _bag.Add(item);
            }
        }

        var list1 = new List<TranslateText>();
        var list2 = new List<CoderReviewText>();

        foreach (var viewText in context.ViewTexts)
        {
            var id = SinkHelper.GetStringHashCode(viewText.Text);

            if (!_bag.Contains(id))
            {
                _bag.Add(id);
                list1.Add(new TranslateText()
                {
                    Id = id,
                    ChineseText = viewText.Text
                });
            }
        }

        foreach (var coderText in context.CoderTexts)
        {
            list2.Add(new CoderReviewText()
            {
                File = context.Path,
                Line = coderText.Line,
                Text = coderText.OriginalText
            });
        }

        if (list1.Any())
        {
            await _context.BulkInsertAsync(list1);
        }

        if (list2.Any())
        {
            await _context.BulkInsertAsync(list2);
        }
    }
}