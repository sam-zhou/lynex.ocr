namespace Lynex.AspNet.Identity
{
	internal sealed class SecurityToken
	{
		private readonly byte[] _data;

		public SecurityToken(byte[] data)
		{
			this._data = (byte[])data.Clone();
		}

		internal byte[] GetDataNoClone()
		{
			return this._data;
		}
	}
}
