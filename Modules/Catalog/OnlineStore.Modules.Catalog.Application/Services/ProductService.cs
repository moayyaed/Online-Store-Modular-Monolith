using System.Threading.Tasks;

namespace OnlineStore.Modules.Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        private const string ProductEntityTypeId = "Product";
        private readonly IEntityService _entityService;

        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository, IEntityService entityService)
        {
            _productRepository = productRepository;
            _entityService = entityService;
        }

        public void Create(Product product)
        {
            using (var transaction = _productRepository.BeginTransaction())
            {
                product.Slug = _entityService.ToSafeSlug(product.Slug, product.Id, ProductEntityTypeId);
                _productRepository.Add(product);
                _productRepository.SaveChanges();

                _entityService.Add(product.Name, product.Slug, product.Id, ProductEntityTypeId);
                _productRepository.SaveChanges();

                transaction.Commit();
            }
        }

        public void Update(Product product)
        {
            var slug = _entityService.Get(product.Id, ProductEntityTypeId);
            if (product.IsVisibleIndividually)
            {
                product.Slug = _entityService.ToSafeSlug(product.Slug, product.Id, ProductEntityTypeId);
                if (slug != null)
                    _entityService.Update(product.Name, product.Slug, product.Id, ProductEntityTypeId);
                else
                    _entityService.Add(product.Name, product.Slug, product.Id, ProductEntityTypeId);
            }
            else
            {
                if (slug != null) _entityService.Remove(product.Id, ProductEntityTypeId);
            }

            _productRepository.SaveChanges();
        }

        public async Task Delete(Product product)
        {
            product.IsDeleted = true;
            await _entityService.Remove(product.Id, ProductEntityTypeId);
            _productRepository.SaveChanges();
        }
    }
}