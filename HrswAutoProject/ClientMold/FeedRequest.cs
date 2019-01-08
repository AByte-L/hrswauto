using Gy.HrswAuto.PLCMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    public class FeedRequest
    {
        public int ClientID { get; set; }
        public CmmClient Client { get; set; }
        
        public virtual void Perform()
        {
            throw new NotImplementedException();
        }
    }

    public class GripFeedRequest : FeedRequest
    {
        public bool IsPassed { get; set; }
        public override void Perform()
        {
            PlcClient.Instance.ResponseGripRequest(ClientID, IsPassed);
        }
    }

    public class PlaceFeedRequest : FeedRequest
    {
        public override void Perform()
        {
            PlcClient.Instance.ResponsePlaceRequest(ClientID);
        }
    }
}
