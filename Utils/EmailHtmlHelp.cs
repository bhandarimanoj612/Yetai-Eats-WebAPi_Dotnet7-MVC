namespace Yetai_Eats.Utils
{
    public class EmailHtmlHelp
    {
        public static string GetHelpEmailBody(string email, string body)
        {
            string imageUrl = "https://img.freepik.com/free-vector/youth-empowerment-abstract-concept-vector-illustration-children-young-people-take-charge-take-action-improve-life-quality-democracy-building-youth-activism-involvement-abstract-metaphor_335657-1925.jpg?size=338&ext=jpg&ga=GA1.1.1448711260.1707091200&semt=ais";

            string response = $@"
            <div style=""text-align: center; background-color: #f6f6f6; font-family: 'Rubik', sans-serif; padding: 0.3rem;"">
                <div style=""max-width: 500px; margin: 0 auto;"">
                    <section>
                        <div style=""margin: 2rem auto; padding: 1rem; background-color: #ffffff; border-radius: 0.5rem; font-family: 'Rubik', sans-serif;"">
                            <h2 style=""color: #555;"">Help Needed</h2>
                            <p style=""line-height: 1.5; font-size: 16px;"">Email: {email}</p>
                            <p style=""line-height: 1.5; font-size: 16px;"">Message:</p>
                            <p style=""line-height: 1.5; font-size: 16px;"">{body}</p>
                            <img src=""{imageUrl}"" alt=""Youth Empowerment"" style=""max-width: 100%; border-radius: 0.5rem; margin-top: 1rem;"">
                        </div>
                    </section>
                </div>
            </div>
            ";

            return response;
        }
    }
}
