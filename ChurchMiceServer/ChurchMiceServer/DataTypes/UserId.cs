namespace ChurchMiceServer.DataTypes;

public class UserId
{
    private string userId;

    public UserId(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("A userId cannot be null or empty");
        }
        userId = id;
    }

    public UserId(UserId that)
    {
        userId = that.userId;
    }

    public override string ToString()
    {
        return userId;
    }
}