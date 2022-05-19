using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure
{
    public class USAddress
    {
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }

        public static USAddress FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            USAddress usAddress = new USAddress();
            usAddress.ZipCode = values[0];
            usAddress.City = values[1];
            usAddress.StateId = values[2];
            usAddress.StateName = values[3];

            return usAddress;
        }
    }
    public class USAddresses
    {

        public List<USAddress> ValidUSAddresses
        {
            get { return File.ReadAllLines(".\\uszips.csv").Skip(1).Select(v => USAddress.FromCsv(v)).ToList(); }
        }
    }
}
