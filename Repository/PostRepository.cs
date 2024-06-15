using API_BLOG.Data;
using API_BLOG.Helpers;
using API_BLOG.Models.Dtos.Post;
using API_BLOG.Models.Entitys;
using API_BLOG.Repository.IRepository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API_BLOG.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public PostRepository(ApplicationDbContext context, IOptions<CloudfinarySettings> config):base(context) 
        {
            _context = context;
            var acc = new Account
                (
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );

            _cloudinary = new Cloudinary( acc );

        }

        public async Task<Post> Update(Post entity)
        {


            entity.UpdateOn =  DateTime.Now;
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<Post> Delete(Post entity)
        {
            entity.UpdateOn = DateTime.Now;
            entity.Status = false;
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ImageUploadResult> UploadImage(PostCreateDto postCreate)
        {

            var uploadresult = new ImageUploadResult();

            if (postCreate.Image.Length > 0)
            {

                using var stream = postCreate.Image.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(postCreate.Image.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadresult = await _cloudinary.UploadAsync(uploadParams);

            }

            return uploadresult;
        }


    }
}
