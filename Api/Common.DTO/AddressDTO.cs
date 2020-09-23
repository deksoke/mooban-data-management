using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class AddressDTO
    {
        public AddressDTO() { }

        public AddressDTO(string city, string street, string zipCode, double? lat, double? lng)
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
            Lat = lat;
            Lng = lng;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
