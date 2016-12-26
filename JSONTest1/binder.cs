using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;
    using System.ServiceModel.Channels;


namespace JSONTest1
{

        public class binder : WebContentTypeMapper
        {
            public override WebContentFormat GetMessageFormatForContentType(string contentType)
            {

                if (contentType.Contains("text/xml") || contentType.Contains("application/xml"))
                {

                    return WebContentFormat.Raw;

                }

                else
                {

                    return WebContentFormat.Default;

                }
            }
        }
    
}