using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.Writing
{
    [XmlElement("Message")]
    public class Message<T>
    {
        [XmlElement(XmlFormNameRule.LowerFirstLetter)]
        public string SpecificTrash { get; set; }

        [XmlElement(XmlFormNameRule.GetAttributeFromTypeDeclaration)]
        public T Transaction { get; set; }
    }

    [XmlElement("Message")]
    public class ArrayMessage<T>
    {
        [XmlElement(XmlFormNameRule.LowerFirstLetter)]
        public string SpecificTrash { get; set; }

        [XmlElement(XmlFormNameRule.GetAttributeFromTypeDeclaration)]
        public T[] Transactions { get; set; }
    }

    [XmlElement(XmlFormNameRule.Default)]
    public class SpecificTransaction
    {
        [XmlElement(XmlFormNameRule.LowerAllLetters)]
        public string SpecificTransactionTrash { get; set; }
    }

    public class ТипСЗВ2010
    {
        public string Zzz { get; set; }
    }

    public class ТипОписьСЗВ2010
    {
        public string Xxx { get; set; }
    }

    public class ТипПачкаВходящихДокументовСЗВ2010
    {
        public ТипПачкаВходящихДокументовСЗВ2010()
        {
            Окружение = "В составе файла";
            Стадия = "До обработки";
        }

        [System.Xml.Serialization.XmlAttribute]
        public string Окружение { get; private set; }

        [System.Xml.Serialization.XmlAttribute]
        public string Стадия { get; private set; }

        public ТипОписьСЗВ2010 ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ { get; set; }
        public ТипСЗВ2010[] СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ { get; set; }
    }

    public class ТипЗаголовокФайла
    {
        public ТипЗаголовокФайла()
        {
            ВерсияФормата = "07.00";
            ТипФайла = "ВНЕШНИЙ";
            ИсточникДанных = "СТРАХОВАТЕЛЬ";
            ПрограммаПодготовкиДанных = new ТипПрограммаПодготовкиДанных();
        }

        public string ВерсияФормата { get; private set; }
        public string ТипФайла { get; private set; }
        public ТипПрограммаПодготовкиДанных ПрограммаПодготовкиДанных { get; private set; }
        public string ИсточникДанных { get; private set; }
    }

    public class ТипПрограммаПодготовкиДанных
    {
        public ТипПрограммаПодготовкиДанных()
        {
            НазваниеПрограммы = "КОНТУР-ОТЧЕТ ПФ";
            Версия = "1.0";
        }

        public string НазваниеПрограммы { get; private set; }
        public string Версия { get; private set; }
    }

    public class ТипДеньги
    {
        public string Value { get; set; }
    }

    public abstract class ФайлПФР
    {
        public string ИмяФайла { get; set; }
        public ТипЗаголовокФайла ЗаголовокФайла { get; set; }
    }

    public class ФайлПФРСЗВ2010 : ФайлПФР
    {
        public ТипПачкаВходящихДокументовСЗВ2010 ПачкаВходящихДокументов { get; set; }
    }
}