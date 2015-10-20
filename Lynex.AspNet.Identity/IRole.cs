namespace Lynex.AspNet.Identity
{
	public interface IRole<out TKey>
	{
		TKey Id
		{
			get;
		}

		string Name
		{
			get;
			set;
		}
	}
	public interface IRole : IRole<string>
	{
	}
}
