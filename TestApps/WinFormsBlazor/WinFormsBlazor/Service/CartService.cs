using WinFormsBlazor.Model;

namespace WinFormsBlazor.Service
{
    public class CartService
    {
        public static List<Cart> SelectedBlogs { get; set; } = new();

        public static Task<List<Cart>> CartItemsAsync()
        {
            return Task.FromResult(SelectedBlogs);
        }

        public static Task<List<Cart>> CartItemsInfo(int id)
        {
            List<Cart> SelectedBlogInfo = SelectedBlogs.FindAll(element => element.BlogId == id);
            return Task.FromResult(SelectedBlogInfo);
        }

        public static void AddToCart(int id, string title, string sTitle, int num, int price)
        {
            bool isUpdate = false;
            if (SelectedBlogs.Count == 0 )
            {
                isUpdate = false;
            }
            else
            {
                for (int i = 0; i < SelectedBlogs.Count; i++)
                {
                    if (SelectedBlogs[i].BlogId == id)
                    {
                        int bNum = SelectedBlogs[i].BlogNum + 1;
                        SelectedBlogs[i].BlogNum = bNum;
                        isUpdate = true;
                    }
                }
            }

            if (isUpdate == false)
            {
                var newCart = new Cart
                {
                    BlogId = id,
                    Title = title,
                    SubTitle = sTitle,
                    BlogNum = num,
                    BlogPrice = price
                };
                SelectedBlogs.Add(newCart);
            }
        }

        public static void CartUpdate(int id, int cnt)
        {
            for (int i = 0; i < SelectedBlogs.Count; i++)
            {
                if (SelectedBlogs[i].BlogId == id)
                {
                    SelectedBlogs[i].BlogNum = cnt;
                }
            }
        }

        public static void CartAllDelItem()
        {
            SelectedBlogs.Clear();
        }

        public static void CartOneDelItem(int id)
        {
            for (int i = 0; i < SelectedBlogs.Count; i++)
            {
                if (SelectedBlogs[i].BlogId == id)
                {
                    SelectedBlogs.Remove(SelectedBlogs[i]);
                }
            }
        }
    }
}