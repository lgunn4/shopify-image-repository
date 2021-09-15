using shopify_image_repository.Models;

namespace shopify_image_repository.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ImageRepositoryContext context)
        {
            context.Database.EnsureCreated();
            
            var users = new User[]
            {
                new User{UserName="Carson"},
                new User{UserName="Meredith"},
                new User{UserName="Arturo"},
                new User{UserName="Gytis"},
                new User{UserName="Yan"},
                new User{UserName="Peggy"},
                new User{UserName="Laura"},
                new User{UserName="Nino"}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}