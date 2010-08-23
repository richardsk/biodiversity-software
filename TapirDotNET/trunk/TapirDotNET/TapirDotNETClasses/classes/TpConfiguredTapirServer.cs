using System;
using System.Collections.Generic;
using System.Text;

namespace TapirDotNET
{
    /// <summary>
    /// Implements a simple configured Tapir Server.  The server data source and mappings are configured using the TapitDotNETAdmin Configurator.
    /// </summary>
    public class TpConfiguredTapirServer : ITapirServer
    {

        #region ITapirServer Members

        public TpResponse ProcessRequest(TpRequest req)
        {
            TpResponse resp = null;

            try
            {
                //standard configurator execution
                string operation = req.GetOperation().ToLower();

                if (operation == "ping")
                {
                    resp = new TpPingResponse(req);
                }
                else if (operation == "capabilities")
                {
                    resp = new TpCapabilitiesResponse(req);
                }
                else if (operation == "metadata")
                {
                    resp = new TpMetadataResponse(req);
                }
                else if (operation == "inventory")
                {
                    resp = new TpInventoryResponse(req);
                }
                else if (operation == "search")
                {
                    resp = new TpSearchResponse(req);
                }
                else
                {
                    // Unknown operation 
                    resp = new TpResponse(req);
                    resp.Init();
                    resp.ReturnError("Unknown operation \"" + operation + "\"");
                }

                resp.Process();
            }
            catch (Exception ex)
            {
                new TpDiagnostics().Append(TpConfigManager.CFG_INTERNAL_ERROR, "ERROR : " + ex.Message, TpConfigManager.DIAG_FATAL);
            }

            return resp;
        }

        #endregion
    }
}
