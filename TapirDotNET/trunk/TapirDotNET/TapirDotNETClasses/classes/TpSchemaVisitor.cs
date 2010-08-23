namespace TapirDotNET 
{

	public class TpSchemaVisitor
	{
		public TpSchemaVisitor()
		{
			
		}
		
		
		public virtual object VisitElementDecl(IXsTypedObject rXsElementDecl, object path)
		{
			return null; 	
		}// end of member function VisitElementDecl
		
		public virtual object VisitAttributeUse(IXsTypedObject rXsAttributeUse, object path)
		{
			return null;
		}// end of member function VisitAttributeUse
		
		public virtual object VisitModelGroup(object rXsModelGroup, object path)
		{
			return null;
		}// end of member function VisitModelGroup
	}
}
