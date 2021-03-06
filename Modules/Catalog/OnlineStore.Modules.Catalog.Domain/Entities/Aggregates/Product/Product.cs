using System;
using System.Collections.Generic;

namespace OnlineStore.Modules.Catalog.Domain.Entities
{
    public class Product : AggregateRoot<ProductId>
    {
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTimeOffset? SpecialPriceStart { get; set; }

        public DateTimeOffset? SpecialPriceEnd { get; set; }

        public bool HasOptions { get; set; }

        public bool IsVisibleIndividually { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public bool StockTrackingIsEnabled { get; set; }

        public int StockQuantity { get; set; }

        public string Sku { get; set; }

        public string BarCode { get; set; }

        public string NormalizedName { get; set; }

        public int DisplayOrder { get; set; }

        public long? SellerId { get; set; }

        public string ImageUrl { get; set; }

        public IList<ProductMedia> Medias { get; protected set; } = new List<ProductMedia>();

        public IList<ProductLink> ProductLinks { get; protected set; } = new List<ProductLink>();

        public IList<ProductLink> LinkedProductLinks { get; protected set; } = new List<ProductLink>();

        public IList<ProductAttributeValue> AttributeValues { get; protected set; } = new List<ProductAttributeValue>();

        public IList<ProductOptionValue> OptionValues { get; protected set; } = new List<ProductOptionValue>();

        public IList<ProductCategory> Categories { get; protected set; } = new List<ProductCategory>();

        public IList<ProductPriceHistory> PriceHistories { get; protected set; } = new List<ProductPriceHistory>();

        public int ReviewsCount { get; set; }

        public double? RatingAverage { get; set; }

        public long? BrandId { get; set; }

        public Brand Brand { get; set; }

        public long? TaxClassId { get; set; }

        public TaxClass TaxClass { get; set; }

        public IList<ProductOptionCombination> OptionCombinations { get; protected set; } =
            new List<ProductOptionCombination>();

        public void AddCategory(ProductCategory category)
        {
            category.Product = this;
            Categories.Add(category);
        }

        public void AddMedia(ProductMedia media)
        {
            media.Product = this;
            Medias.Add(media);
        }

        public void AddAttributeValue(ProductAttributeValue attributeValue)
        {
            attributeValue.Product = this;
            AttributeValues.Add(attributeValue);
        }

        public void AddOptionValue(ProductOptionValue optionValue)
        {
            optionValue.Product = this;
            OptionValues.Add(optionValue);
        }

        public void AddProductLinks(ProductLink productLink)
        {
            productLink.Product = this;
            ProductLinks.Add(productLink);
        }

        public void AddOptionCombination(ProductOptionCombination combination)
        {
            combination.Product = this;
            OptionCombinations.Add(combination);
        }
    }
}