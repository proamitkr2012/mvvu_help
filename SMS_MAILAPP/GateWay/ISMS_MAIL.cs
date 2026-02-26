namespace SMS_MAIL.GateWay
{
    public interface ISMS_MAIL
    {

        public Task<int> sendSMS(string mobile, string msg, string templateID);
        public Task<int> sendSMSHindi(string mobile, string msg, string templateID);

        public Task<int> sendMail(string emailId, string subject, string msg);
    }
}
