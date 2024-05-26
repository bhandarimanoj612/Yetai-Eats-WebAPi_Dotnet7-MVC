namespace Yetai_Eats.Utils
{
    public class EmailHtmlContent
    {
        public string AccountRegistrationResponse(string sellerName)
        {
            string response = $@"
            <div style=""text-align: center; background-color: #f6f6f6; font-family: 'Rubik', sans-serif; padding: 0.3rem;"">
                <div style=""max-width: 500px; margin: 0 auto;"">
                    <section>
                        <div style=""margin: 2rem auto; padding: 1rem; background-color: #ffffff; border-radius: 0.5rem; font-family: 'Rubik', sans-serif;"">
                            <div style=""position: relative;"">
                                <h2 style=""position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); color: #555; font-family: 'Rubik', sans-serif;"">Yetai Eats Seller</h2>
                                <img src=""https://scontent.fjkr2-1.fna.fbcdn.net/v/t39.30808-6/442509384_1919556655146089_6053559135406766083_n.jpg?_nc_cat=101&ccb=1-7&_nc_sid=5f2048&_nc_eui2=AeE6Z1xJciU8qLJUbvaFz-YB1_pkeIlEj4LX-mR4iUSPgmlM9lHI10pStbQBVZwwcQLH1IZvwNMGTm9IliDXEAkE&_nc_ohc=MvctORU03PcQ7kNvgGIJzd1&_nc_ht=scontent.fjkr2-1.fna&oh=00_AYCcllcwASfrQZCO3aG-gkoVt4e84H5IkQRYg0FUzpubmQ&oe=664D4158"" style=""width: 100%; border-radius: 0.5rem; opacity:0.7;"">
                            </div>
                            <p style=""margin-top: 1.5rem; text-align: left; font-size: 16px;"">Hey {sellerName},</p>
                            <p style=""line-height: 1.5; text-align: left; font-size: 16px;"">You're just one step away from managing your delicious offerings with ease. Access your seller dashboard and start delighting customers.</p>
                            <div style=""text-align: center;"">
                                <a href=""http://localhost:3000/"" style=""display: inline-block; padding: 0.6rem 1.1rem; background-color: #28a745; color: #fff; text-decoration: none; font-size: 16px; font-weight: bold; border-radius: 0.3rem;"">Go to Login Page</a>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
            ";

            return response;
        }

    }
}
