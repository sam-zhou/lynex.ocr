namespace Lynex.AspNet.Identity
{
	public interface IUser<out TKey>
	{
		TKey Id
		{
			get;
		}

		string UserName
		{
			get;
			set;
		}
	}
	public interface IUser : IUser<string>
	{
	}
}
