using System;
using System.Threading.Tasks;
using Hugin.BookStore.Enums;
using Hugin.Domain.Entities;
using Hugin.Domain.Repository;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiStadium;

namespace Hugin.BookStore
{
    public class DataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<BookShop, Guid> _bookShopRepository;
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<BookInBookShop, Guid> _bookInBookShopRepository;
        private readonly IRepository<BookShopOwner, Guid> _bookShopOwnerRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorManager _authorManager;
        private readonly IGuidGenerator _guidGenerator;

        public DataSeederContributor(
            IDataFilter dataFilter,
            IRepository<BookShop, Guid> bookShopRepository,
            IRepository<Book, Guid> bookRepository,
            IRepository<BookInBookShop, Guid> bookInBookShopRepository,
            IRepository<BookShopOwner, Guid> bookShopOwnerRepository,
            IAuthorRepository authorRepository,
            IAuthorManager authorManager,
            IGuidGenerator guidGenerator)
        {
            _dataFilter = dataFilter;
            _bookShopRepository = bookShopRepository;
            _bookRepository = bookRepository;
            _bookInBookShopRepository = bookInBookShopRepository;
            _bookShopOwnerRepository = bookShopOwnerRepository;
            _authorRepository = authorRepository;
            _authorManager = authorManager;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            /*
             *  除了第一次部署时的数据初始化，有关当前模块的初始数据都应该主动种子
             *
             *  trySetId 不是必须的，但是一个比较好的方式去解决关联问题
             *  当然关联时可以是Guid字符串或者对象，但可能需要指定好如Code类唯一性的Index
             */

            using (_dataFilter.Disable<IMultiUser>())
            {
                using (_dataFilter.Disable<IMultiStadium>())
                {
                    //BookShopOwner
                    await _bookShopOwnerRepository.InsertOnlyNotExistAsync(new BookShopOwner
                    {
                        Name = "马云",
                    }.TrySetId(new Guid("5aebb468-9d50-4eaa-a6cc-dec91f8a291b")), autoSave: true);

                    await _bookShopOwnerRepository.InsertOnlyNotExistAsync(new BookShopOwner
                    {
                        Name = "马化腾",
                    }.TrySetId(new Guid("4868fa69-404d-4091-9b68-e2c7b390c9b6")), autoSave: true);

                    await _bookShopOwnerRepository.InsertOnlyNotExistAsync(new BookShopOwner
                    {
                        Name = "刘强东",
                    }.TrySetId(new Guid("b3a23042-9786-45b1-82de-f3f146040f88")), autoSave: true);

                    //BookShop
                    if (!(await _bookShopRepository.AnyAsync(p => p.Code.Equals(BookShopType.TaoBao))))
                    {
                        await _bookShopRepository.InsertAsync(new BookShop(BookShopType.TaoBao, "淘宝")
                        {
                            OwnerId = new Guid("5aebb468-9d50-4eaa-a6cc-dec91f8a291b"),
                        }.TrySetId(new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")));
                    }

                    if (!(await _bookShopRepository.AnyAsync(p => p.Code.Equals(BookShopType.JingDong))))
                    {
                        await _bookShopRepository.InsertAsync(new BookShop(BookShopType.JingDong, "京东")
                        {
                            OwnerId = new Guid("b3a23042-9786-45b1-82de-f3f146040f88"),
                            IsStop = true
                        }.TrySetId(new Guid("65ae0986-c50f-4100-b890-807f8f9b44b3"))
                        .TrySetUserId(new Guid("2e701e62-0953-4dd3-910b-dc6cc93ccb0d")));
                    }

                    if (!(await _bookShopRepository.AnyAsync(p => p.Code.Equals(BookShopType.DangDang))))
                    {
                        await _bookShopRepository.InsertAsync(new BookShop(BookShopType.DangDang, "当当网").TrySetId(new Guid("77e523d4-8625-4a7d-94c3-1aa25b3765ee")));
                    }

                    //Author
                    if (await _authorRepository.GetCountAsync() <= 0)
                    {
                        //这种方式不好
                        await _authorRepository.InsertAsync(
                            (await _authorManager.CreateAsync(
                                name: "George Orwell",
                                birthDate: new DateTime(1903, 06, 25),
                                profile: "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                            )).TrySetId(new Guid("6b442554-f912-4e0e-b8dd-ee996d4fc7f6"))
                        );

                        await _authorRepository.InsertAsync(
                            (await _authorManager.CreateAsync(
                                "Douglas Adams",
                                new DateTime(1952, 03, 11),
                                "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                            )).TrySetId(new Guid("162db671-daf2-42f4-81b8-edb3ecb05bb8"))
                        );
                    }

                    //Book
                    await _bookRepository.InsertOnlyNotExistAsync(new Book
                    {
                        Name = "1984",
                        BookType = BookType.Adventure,
                        PublishDate = new DateTime(1949, 6, 8),
                        Price = 19.84m,
                        AuthorId = new Guid("6b442554-f912-4e0e-b8dd-ee996d4fc7f6")
                    }.TrySetId(new Guid("0FC4CFF5-84A5-1EBB-5BE2-39F9C9E3E5D9")), autoSave: true);

                    await _bookRepository.InsertOnlyNotExistAsync(new Book
                    {
                        Name = "The Hitchhiker's Guide to the Galaxy",
                        BookType = BookType.Fantastic,
                        PublishDate = new DateTime(1995, 9, 27),
                        Price = 42.0m
                    }.TrySetId(new Guid("fb0b0f0e-01cc-575f-a1c5-39f9c8d30392")), autoSave: true);

                    await _bookRepository.InsertOnlyNotExistAsync(new Book
                    {
                        Name = "剑来",
                        BookType = BookType.Fantastic,
                        PublishDate = new DateTime(2019, 1, 1),
                        Price = 65.0m
                    }.TrySetId(new Guid("53ade93e-5b6e-407c-a0d2-9ed0c05eda29")), autoSave: true);

                    await _bookRepository.InsertOnlyNotExistAsync(new Book
                    {
                        Name = "大道朝天",
                        BookType = BookType.Adventure,
                        PublishDate = new DateTime(2019, 1, 1),
                        Price = 75.0m
                    }.TrySetId(new Guid("030ab510-dab3-45bf-9442-899fabaa2bfc")), autoSave: true);

                    //BookInBookShop
                    if (!(await _bookInBookShopRepository.AnyAsync(p =>
                        p.BookId.Equals(new Guid("0FC4CFF5-84A5-1EBB-5BE2-39F9C9E3E5D9"))
                        && p.BookShopId.Equals(new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")))))
                    {
                        await _bookInBookShopRepository.InsertAsync(new BookInBookShop
                        {
                            BookId = new Guid("0FC4CFF5-84A5-1EBB-5BE2-39F9C9E3E5D9"),
                            BookShopId = new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")
                        });
                    }

                    if (!(await _bookInBookShopRepository.AnyAsync(p =>
                        p.BookId.Equals(new Guid("fb0b0f0e-01cc-575f-a1c5-39f9c8d30392"))
                        && p.BookShopId.Equals(new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")))))
                    {
                        await _bookInBookShopRepository.InsertAsync(new BookInBookShop
                        {
                            BookId = new Guid("fb0b0f0e-01cc-575f-a1c5-39f9c8d30392"),
                            BookShopId = new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")
                        });
                    }

                    if (!(await _bookInBookShopRepository.AnyAsync(p =>
                        p.BookId.Equals(new Guid("53ade93e-5b6e-407c-a0d2-9ed0c05eda29"))
                        && p.BookShopId.Equals(new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")))))
                    {
                        await _bookInBookShopRepository.InsertAsync(new BookInBookShop
                        {
                            BookId = new Guid("53ade93e-5b6e-407c-a0d2-9ed0c05eda29"),
                            BookShopId = new Guid("9cfc821e-a1c3-4a84-8130-1970e6441279")
                        });
                    }

                    if (!(await _bookInBookShopRepository.AnyAsync(p =>
                        p.BookId.Equals(new Guid("030ab510-dab3-45bf-9442-899fabaa2bfc"))
                        && p.BookShopId.Equals(new Guid("65ae0986-c50f-4100-b890-807f8f9b44b3")))))
                    {
                        await _bookInBookShopRepository.InsertAsync(new BookInBookShop
                        {
                            BookId = new Guid("030ab510-dab3-45bf-9442-899fabaa2bfc"),
                            BookShopId = new Guid("65ae0986-c50f-4100-b890-807f8f9b44b3")
                        });
                    }
                }
            }
        }
    }
}
