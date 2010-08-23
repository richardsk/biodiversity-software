namespace TapirDotNET 
{

	public class XsSimpleType:XsType
	{
		
		public XsSimpleType(string name, string targetNamespace, string defaultNamespace, bool isGlobal):base(name, targetNamespace, defaultNamespace, isGlobal)
		{
			
			this.mIsSimple = true;
		}
		
        public string GetUri( ) 
        {
            return this.GetTargetNamespace() + "#" + this.GetName();
            
        } // end of member function GetUri
	}
}
