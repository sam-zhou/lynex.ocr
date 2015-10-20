namespace Lynex.AspNet.Identity
{
	public class IdentityMessage
	{
		public virtual string Destination
		{
			get;
			set;
		}

		public virtual string Subject
		{
			get;
			set;
		}

		public virtual string Body
		{
			get;
			set;
		}
	}
}
