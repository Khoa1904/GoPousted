using Microsoft.EntityFrameworkCore;
using WinFormsBlazor.Model;

namespace WinFormsBlazor.Service
{
    public class BlogService
    {
        //  https://learn.microsoft.com/en-us/ef/core/querying/sql-queries
        //  https://www.codeproject.com/Articles/5321286/Executing-Raw-SQL-Queries-using-Entity-Framework-C
        public static Task<List<Blog>> GetBlogAsync()
        {
            /*
            var dbContext = new BlogDbContext();
            var query = dbContext.Blogs.OrderByDescending(a => a.BlogId);
            return query.ToListAsync();
            */

            FormattableString Query = @$"
                                        SELECT BlogId, Title, SubTitle, BlogNum, DateTimeAdd
                                        FROM Blogs
                                        ORDER BY BlogId DESC
                                        ";
            var dbContext = new BlogDbContext();
            var query = dbContext.Blogs.FromSql(Query);
            return query.ToListAsync();
        }

        public static Task<List<Blog>> GetBlogInfo(int id)
        {
            FormattableString Query = @$"
                                        SELECT BlogId, Title, SubTitle, BlogNum, DateTimeAdd
                                        FROM Blogs
                                        WHERE BlogId = {id}
                                        ";
            var dbContext = new BlogDbContext();
            var query = dbContext.Blogs.FromSql(Query);
            return query.ToListAsync();
        }

        public static async Task AddBlog(string title, string subTitle)
        {
            try
            {
                /*
                var newBlog = new Blog {
                    Title = title,
                    SubTitle = subTitle,
                    BlogNum = 1,
                    DateTimeAdd = DateTime.Today
                };

                var dbContext = new BlogDbContext();
                dbContext.Add(newBlog);
                await dbContext.SaveChangesAsync();
                */

                FormattableString Query = @$"
                                            INSERT INTO Blogs
                                                (Title, SubTitle, BlogNum, DateTimeAdd) VALUES
                                                ({title},{subTitle},'1',{DateTime.Today})
                                            ";
                var dbContext = new BlogDbContext();
                dbContext.Database.ExecuteSql(Query);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task UpdateBlog(int id, int cnt)
        {
            try
            {
                /*
                var dbContext = new BlogDbContext();
                var blog = dbContext.Blogs.First(a => a.BlogId == id);
                blog.BlogNum = cnt;
                await dbContext.SaveChangesAsync();
                */

                FormattableString Query = @$"
                                            UPDATE Blogs SET
                                                BlogNum     =   {cnt}
                                            WHERE BlogId    =   {id}
                                            ";
                var dbContext = new BlogDbContext();
                dbContext.Database.ExecuteSql(Query);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task DeleteBlog(int id)
        {
            try
            {
                /*
                var dbContext = new BlogDbContext();
                var blog = new Blog { BlogId = id };
                dbContext.Remove(blog);
                await dbContext.SaveChangesAsync();
                 */

                FormattableString Query = @$"
                                            DELETE FROM Blogs WHERE BlogId = {id}
                                            ";
                var dbContext = new BlogDbContext();
                dbContext.Database.ExecuteSql(Query);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}