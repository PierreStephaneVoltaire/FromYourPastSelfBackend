using System;
using System.Collections.Generic;
using System.Text;
using infrastructure;

namespace Domain.Adapter
{
   public interface Mail
    {
        void sendEmail(MessageFromPastSelf messageFromPastSelf);
        string scheduleEmail(MessageFromPastSelf messageFromPastSelf);
        string updateEmail(MessageFromPastSelf messageFromPastSelf);
        bool deleteEmail(MessageFromPastSelf messageFromPastSelf);
    }
}
