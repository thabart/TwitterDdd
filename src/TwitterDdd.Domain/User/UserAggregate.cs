namespace TwitterDdd.Domain.User
{
    public interface IUserAggregate
    {
        UserState Create(string subject);
    }

    public class UserAggregate : IUserAggregate
    {
        public UserState Create(string subject)
        {
            return new UserState
            {
                Subject = "subject"
            };
        }
    }
}
