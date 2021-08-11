using System.Collections.Generic;
using System.Xml;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    public class ConfigurationReader : System.Xml.Serialization.XmlSerializationReader
    {
        public object Read()
        {
            Reader.MoveToContent();
            if (Reader.NodeType == XmlNodeType.Element)
            {
                if ((Reader.LocalName.Equals(idRoot) && Reader.NamespaceURI.Equals(idItem)))
                {
                    return ReadConfiguration(true, true);
                }
                throw CreateUnknownNodeException();
            }
            
            UnknownNode(null, @":LSOne");
            return null;
        }

        Configuration ReadConfiguration(bool isNullable, bool checkType)
        {
            XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || (/*xsiType.Name == (object)id3_DynaFormsSettings &&*/ xsiType.Namespace.Equals(idItem)))
                {
                }
                else
                    throw CreateUnknownTypeException(xsiType);
            }
            if (isNull) return null;

            var o = new Configuration();

            if (o.Providers == null)
                o.Providers = new List<Provider>();
            
            /*bool[] paramsRead = new bool[32];
            while (Reader.MoveToNextAttribute())
            {
                if (!paramsRead[30] && ((object)Reader.LocalName == (object)id4_type && (object)Reader.NamespaceURI == (object)idItem))
                {
                    o.@Type = Reader.Value;
                    paramsRead[30] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o, @":type");
                }
            }*/

            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }

            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != XmlNodeType.EndElement && Reader.NodeType != XmlNodeType.None)
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    if ((object)Reader.LocalName == (object)idRegisterDefaults && (object)Reader.NamespaceURI == (object)idItem)
                    {
                        o.RegisterDefaultProviders = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                    }
                    else if (Reader.LocalName.Equals(idProviders) && Reader.NamespaceURI.Equals(idItem))
                    {
                        if (!ReadNull())
                        {
                            if ((object) (o.Providers) == null)
                                o.Providers = new List<Provider>();
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations4 = 0;
                                int readerCount4 = ReaderCount;
                                while (Reader.NodeType != XmlNodeType.EndElement && Reader.NodeType != XmlNodeType.None)
                                {
                                    if (Reader.NodeType == XmlNodeType.Element)
                                    {
                                        if (Reader.LocalName.Equals(idProvider) && Reader.NamespaceURI.Equals(idItem))
                                        {
                                            o.Providers.Add(ReadProvider(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":Provider");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":Provider");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                }
                else
                {
                    UnknownNode((object) o, ":Provider");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        Provider ReadProvider(bool isNullable, bool checkType)
        {
            XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || (xsiType.Name.Equals(idProvider) && xsiType.Namespace.Equals(idItem)))
                {
                }
                else
                    throw CreateUnknownTypeException(xsiType);
            }

            if (isNull) return null;

            var provider = new Provider();
            var paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute())
            {
                if (!paramsRead[0] && (Reader.LocalName.Equals(idAssembly) && Reader.NamespaceURI.Equals(idItem)))
                {
                    provider.Assembly = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && (Reader.LocalName.Equals(idInterface) && Reader.NamespaceURI.Equals(idItem)))
                {
                    provider.Interface = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && (Reader.LocalName.Equals(idImplementation) && Reader.NamespaceURI.Equals(idItem)))
                {
                    provider.Implementation = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)provider, @":assembly, :interface, :implementation");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return provider;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != XmlNodeType.EndElement && Reader.NodeType != XmlNodeType.None)
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    UnknownNode((object)provider, @"");
                }
                else
                {
                    UnknownNode((object)provider, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            ReadEndElement();
            return provider;
        }

        protected override void InitCallbacks()
        {
        }

        string idItem;
        string idProviders;
        string idInterface;
        string idImplementation;
        string idRoot;
        string idProvider;
        string idAssembly;
        string idRegisterDefaults;

        protected override void InitIDs()
        {
            idRoot = Reader.NameTable.Add(@"LSOne");
            idItem = Reader.NameTable.Add(@"");

            idRegisterDefaults = Reader.NameTable.Add(@"RegisterDefaultProviders");

            idProviders = Reader.NameTable.Add(@"Providers");
            idProvider = Reader.NameTable.Add(@"Provider");
            idAssembly = Reader.NameTable.Add(@"assembly");
            idInterface = Reader.NameTable.Add(@"interface");
            idImplementation = Reader.NameTable.Add(@"implementation");
        }
    }
}
