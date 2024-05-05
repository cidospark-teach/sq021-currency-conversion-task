using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace WebApplication3.Models.DTOs
{
    public class ConversionResponse
    {
        public Meta Meta { get; set; }
        public Data Data { get; set; }
    }

    public class Meta
    {
        public string last_updated_at { get; set; }
    }

    public class Data
    {
        public Attr ADA { get; set; }
        public Attr AED { get; set; }
        public Attr AFN { get; set; }
        public Attr ALL { get; set; }
        public Attr AMD { get; set; }
        public Attr ANG { get; set; }
        public Attr AOA { get; set; }
        public Attr ARB { get; set; }
        public Attr ARS { get; set; }
        public Attr AUD { get; set; }
        public Attr AVAX { get; set; }
        public Attr AWG { get; set; }
        public Attr AZN { get; set; }
        public Attr BAM { get; set; }
        public Attr BBD { get; set; }
        public Attr BDT { get; set; }
        public Attr BGN { get; set; }
        public Attr BHD { get; set; }
        public Attr BIF { get; set; }
        public Attr BMD { get; set; }
        public Attr BNB { get; set; }
        public Attr BND { get; set; }
        public Attr BOB { get; set; }
        public Attr BRL { get; set; }
        public Attr BSD { get; set; }
        public Attr BTC { get; set; }
        public Attr BTN { get; set; }
        public Attr BWP { get; set; }
        public Attr BYN { get; set; }
        public Attr BYR { get; set; }
        public Attr BZD { get; set; }
        public Attr CAD { get; set; }
        public Attr CDF { get; set; }
        public Attr CHF { get; set; }
        public Attr CLF { get; set; }
        public Attr CLP { get; set; }
        public Attr CNY { get; set; }
        public Attr COP { get; set; }
        public Attr CRC { get; set; }
        public Attr CUC { get; set; }
        public Attr CUP { get; set; }
        public Attr CVE { get; set; }
        public Attr CZK { get; set; }
        public Attr DAI { get; set; }
        public Attr DJF { get; set; }
        public Attr DKK { get; set; }
        public Attr DOP { get; set; }
        public Attr DOT { get; set; }
        public Attr DZD { get; set; }
        public Attr EGP { get; set; }
        public Attr ERN { get; set; }
        public Attr ETB { get; set; }
        public Attr ETH { get; set; }
        public Attr EUR { get; set; }
        public Attr FJD { get; set; }
        public Attr FKP { get; set; }
        public Attr GBP { get; set; }
        public Attr GEL { get; set; }
        public Attr GGP { get; set; }
        public Attr GHS { get; set; }
        public Attr GIP { get; set; }
        public Attr GMD { get; set; }
        public Attr GNF { get; set; }
        public Attr GTQ { get; set; }
        public Attr GYD { get; set; }
        public Attr HKD { get; set; }
        public Attr HNL { get; set; }
        public Attr HRK { get; set; }
        public Attr HTG { get; set; }
        public Attr HUF { get; set; }
        public Attr IDR { get; set; }
        public Attr ILS { get; set; }
        public Attr IMP { get; set; }
        public Attr INR { get; set; }
        public Attr IQD { get; set; }
        public Attr IRR { get; set; }
        public Attr ISK { get; set; }
        public Attr JEP { get; set; }
        public Attr JMD { get; set; }
        public Attr JOD { get; set; }
        public Attr JPY { get; set; }
        public Attr KES { get; set; }
        public Attr KGS { get; set; }
        public Attr KHR { get; set; }
        public Attr KMF { get; set; }
        public Attr KPW { get; set; }
        public Attr KRW { get; set; }
        public Attr KWD { get; set; }
        public Attr KYD { get; set; }
        public Attr KZT { get; set; }
        public Attr LAK { get; set; }
        public Attr LBP { get; set; }
        public Attr LKR { get; set; }
        public Attr LRD { get; set; }
        public Attr LSL { get; set; }
        public Attr LTC { get; set; }
        public Attr LTL { get; set; }
        public Attr LVL { get; set; }
        public Attr LYD { get; set; }
        public Attr MAD { get; set; }
        public Attr MATIC { get; set; }
        public Attr MDL { get; set; }
        public Attr MGA { get; set; }
        public Attr MKD { get; set; }
        public Attr MMK { get; set; }
        public Attr MNT { get; set; }
        public Attr MOP { get; set; }
        public Attr MRO { get; set; }
        public Attr MRU { get; set; }
        public Attr MUR { get; set; }
        public Attr MVR { get; set; }
        public Attr MWK { get; set; }
        public Attr MXN { get; set; }
        public Attr MYR { get; set; }
        public Attr MZN { get; set; }
        public Attr NAD { get; set; }
        public Attr NGN { get; set; }
        public Attr NIO { get; set; }
        public Attr NOK { get; set; }
        public Attr NPR { get; set; }
        public Attr NZD { get; set; }
        public Attr OMR { get; set; }
        public Attr OP { get; set; }
        public Attr PAB { get; set; }
        public Attr PEN { get; set; }
        public Attr PGK { get; set; }
        public Attr PHP { get; set; }
        public Attr PKR { get; set; }
        public Attr PLN { get; set; }
        public Attr PYG { get; set; }
        public Attr QAR { get; set; }
        public Attr RON { get; set; }
        public Attr RSD { get; set; }
        public Attr RUB { get; set; }
        public Attr RWF { get; set; }
        public Attr SAR { get; set; }
        public Attr SBD { get; set; }
        public Attr SCR { get; set; }
        public Attr SDG { get; set; }
        public Attr SEK { get; set; }
        public Attr SGD { get; set; }
        public Attr SHP { get; set; }
        public Attr SLL { get; set; }
        public Attr SOL { get; set; }
        public Attr SOS { get; set; }
        public Attr SRD { get; set; }
        public Attr STD { get; set; }
        public Attr STN { get; set; }
        public Attr SVC { get; set; }
        public Attr SYP { get; set; }
        public Attr SZL { get; set; }
        public Attr THB { get; set; }
        public Attr TJS { get; set; }
        public Attr TMT { get; set; }
        public Attr TND { get; set; }
        public Attr TOP { get; set; }
        public Attr TRY { get; set; }
        public Attr TTD { get; set; }
        public Attr TWD { get; set; }
        public Attr TZS { get; set; }
        public Attr UAH { get; set; }
        public Attr UGX { get; set; }
        public Attr USD { get; set; }
        public Attr USDC { get; set; }
        public Attr USDT { get; set; }
        public Attr UYU { get; set; }
        public Attr UZS { get; set; }
        public Attr VEF { get; set; }
        public Attr VES { get; set; }
        public Attr VND { get; set; }
        public Attr VUV { get; set; }
        public Attr WST { get; set; }
        public Attr XAF { get; set; }
        public Attr XAG { get; set; }
        public Attr XAU { get; set; }
        public Attr XCD { get; set; }
        public Attr XDR { get; set; }
        public Attr XOF { get; set; }
        public Attr XPD { get; set; }
        public Attr XPF { get; set; }
        public Attr XPT { get; set; }
        public Attr XRP { get; set; }
        public Attr YER { get; set; }
        public Attr ZAR { get; set; }
        public Attr ZMK { get; set; }
        public Attr ZMW { get; set; }
        public Attr ZWL { get; set; }
    }

    public struct Attr
    {
        public string code { get; set; }
        public string value { get; set; }
    }
}
