namespace Yetai_Eats.Utils
{
    public class EmailHtmlContentRider
    {
        public string AccountRegistrationResponse(string sellerName)
        {
            string response = $@"
            <div style=""text-align: center; background-color: #f6f6f6; font-family: 'Rubik', sans-serif; padding: 0.3rem;"">
                <div style=""max-width: 500px; margin: 0 auto;"">
                    <section>
                        <div style=""margin: 2rem auto; padding: 1rem; background-color: #ffffff; border-radius: 0.5rem; font-family: 'Rubik', sans-serif;"">
                            <div style=""position: relative;"">
                                <h2 style=""position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); color: #555; font-family: 'Rubik', sans-serif;"">YetaiEats Rider</h2>
                                <img src=""https://scontent.fjkr2-1.fna.fbcdn.net/v/t39.30808-6/441578323_1919558418479246_8707978640066195770_n.jpg?stp=cp6_dst-jpg&_nc_cat=110&ccb=1-7&_nc_sid=5f2048&_nc_eui2=AeEUa4P6ySH-GWP7w1l087_I5b8QL3VZ-oHlvxAvdVn6gSewlpt_imAK_0S5HKB21QwotcYLebZNYGlR3B31LxEN&_nc_ohc=cP2Bs4aTjWYQ7kNvgGI9VLM&_nc_ht=scontent.fjkr2-1.fna&oh=00_AYDE7KnCvIojMeQm0Y4R0exs30aQF_VwmWf4fLJpAAmwjw&oe=664D4903"" style=""width: 100%; border-radius: 0.5rem; opacity:0.7;"">
                            </div>
                            <p style=""margin-top: 1.5rem; text-align: left; font-size: 16px;"">Hello {sellerName},</p>
                            <p style=""line-height: 1.5; text-align: left; font-size: 16px;"">Welcome to YetaiEats .You're just one step away from managing YetaiEats Rider Application.</p>
                           
                        </div>
                    </section>
                </div>
            </div>
            ";

            return response;
        }
    }
}
