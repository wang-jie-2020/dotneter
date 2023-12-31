﻿using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hugin.BookStore.EntityFrameworkCore
{
    public class BookStoreModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BookStoreModelBuilderConfigurationOptions([NotNull] string tablePrefix = "", [CanBeNull] string schema = null)
            : base(tablePrefix, schema)
        {

        }
    }
}