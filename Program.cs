namespace JsonProject
{
    //importing necessary libraries, also install the packages first
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Sqlite;
    using Newtonsoft.Json;


    // Define a User class to represent user data.
    public class User
    {
        // Properties to store user information.
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public Company Company { get; set; }
    }
    // Define a Company class as a part of User data.
    [Owned]
    public class Company
    {
        // Properties to store company information.
        public string? Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }

    // Define a Post class to represent post data.
    public class Post
    {
        // Properties to store post information.
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }

    // Define a Comment class to represent comment data.
    public class Comment
    {
        // Properties to store comment information.
        public int Id { get; set; }
        public string? PostId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Body { get; set; }

        // Add a Required attribute to enforce the CreatedDateTime property.
        [Required]
        public DateTime CreatedDateTime { get; set; }
    }

    // Define a DbContext for the application.
    public class AppDbContext : DbContext
    {
        // Define DbSet properties for Users, Posts, and Comments.
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Configure the database connection. (SQLite)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=DESKTOP-QMGNP0P;Database=JsonDatabase;User Id=sa;Password=pero99;Encrypt=False");
            // Use SQLite as the database provider.
            optionsBuilder.UseSqlite("Data Source=JsonDatabase.db");
        }

        // Override SaveChangesAsync to set CreatedDateTime for comments.
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // Get the Malaysia time zone
            TimeZoneInfo malaysiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kuala_Lumpur");

            // Find all entities of type Comment that are Added or Modified
            var modifiedComments = ChangeTracker.Entries<Comment>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var comment in modifiedComments)
            {
                // Set the CreatedDateTime property to the current time in Malaysia time zone
                comment.CreatedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, malaysiaTimeZone);
            }

            // Save changes to the database.
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }

    //main program that run the codes in order
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a DbContext instance.
            using (var context = new AppDbContext())
            {
                // Set the time zone for the database connection
                context.Database.ExecuteSqlRaw("PRAGMA timezone = 'Asia/Kuala_Lumpur';");

                // Apply database migrations.
                await context.Database.MigrateAsync();

                // Define URLs for users,posts,comments from JSONPlaceholder API endpoints.
                var userUrl = "https://jsonplaceholder.typicode.com/users";
                var postUrl = "https://jsonplaceholder.typicode.com/posts";
                var commentUrl = "https://jsonplaceholder.typicode.com/comments";

                // Create an HttpClient for making HTTP requests.
                var httpClient = new HttpClient();

                try
                {
                    // Fetch comments from the API.
                    var commentResponse = await httpClient.GetStringAsync(commentUrl);
                    // Deserialize and add comments to the database.
                    var comments = JsonConvert.DeserializeObject<Comment[]>(commentResponse);
                    await context.Comments.AddRangeAsync(comments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"{comments.Length} comments added to the database.");

                    // Fetch users from the API.
                    var userResponse = await httpClient.GetStringAsync(userUrl);
                    // Deserialize and add users to the database.
                    var users = JsonConvert.DeserializeObject<User[]>(userResponse);
                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"{users.Length} users added to the database.");

                    // Fetch posts from the API.
                    var postResponse = await httpClient.GetStringAsync(postUrl);
                    // Log the raw JSON data for retrieving all posts
                    Console.WriteLine("Retrieving all posts from the API");
                    Console.WriteLine(postResponse);
                    // Deserialize and add posts to the database.
                    var posts = JsonConvert.DeserializeObject<Post[]>(postResponse);
                    await context.Posts.AddRangeAsync(posts);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Retrieved all {posts.Length} posts and added to the database.");

                }

                // This block is executed if an exception occurs in the try block.
                catch (Exception ex)
                {
                    // Print an error message to the console, including the exception message.
                    Console.WriteLine("Error: " + ex.Message);

                    // Rethrow the same exception to propagate it further up the call stack.
                    throw;
                }
            }
        }
    }
}