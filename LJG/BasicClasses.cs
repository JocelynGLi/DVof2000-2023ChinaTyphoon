using LJG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XGIS
{
    public class XThematic
    {
        //线实体显示样式
        public Pen LinePen = new Pen(Color.Black, 1);
        //面实体显示样式
        public Pen PolygonPen = new Pen(Color.Red, 1);
        public SolidBrush PolygonBrush = new SolidBrush(Color.FromArgb(255,255,204,204));
        public SolidBrush PolygonBrush1 = new SolidBrush(Color.FromArgb(255,255, 167, 156));
        public SolidBrush PolygonBrush2 = new SolidBrush(Color.FromArgb(255,252, 132, 111));
        public SolidBrush PolygonBrush3 = new SolidBrush(Color.FromArgb(255, 245, 97, 71));
        public SolidBrush PolygonBrush4 = new SolidBrush(Color.FromArgb(255,232, 60, 38));
        public SolidBrush PolygonBrush5 = new SolidBrush(Color.FromArgb(255,219, 0, 1));
        //点实体显示样式
        public Pen PointPen = new Pen(Color.Black, 1);
        public SolidBrush PointBrush = new SolidBrush(Color.Black);
        public int PointRadius = 3;

        public XThematic()
        {
        }
        public XThematic(Pen _LinePen,
            Pen _PolygonPen, SolidBrush _PolygonBrush,
            Pen _PointPen, SolidBrush _PointBrush, int _PointRadius)
        {
            LinePen = _LinePen;
            PolygonPen = _PolygonPen;
            PolygonBrush = _PolygonBrush;
            PointPen = _PointPen;
            PointBrush = _PointBrush;
            PointRadius = _PointRadius;
        }
    }

    public class SelectResult
    {
        public XFeature feature;
        public double criterion;

        public SelectResult(XFeature _feature, double _criterion)
        {
            feature = _feature;
            criterion = _criterion;
        }
    }

    public class XSelect
    {
        public enum OPERATOR
        {
            Equal=0, LessThan=1, MoreThan=2,
            LessEqual=3, MoreEqual=4, Has=5, NotEqual=6
        }

        public static List<XFeature> SelectFeaturesByAttribute(List<XFeature> features,
           OPERATOR op, int fieldIndex, object key)
        {
            List<XFeature> fs = new List<XFeature>();
            foreach (XFeature f in features)
            {
                object value = f.GetAttribute(fieldIndex);
                if (CompareValue(value, op, key))
                    fs.Add(f);
            }
            return fs;
        }

        public static bool CompareValue(object value, OPERATOR op, object key)
        {
            if (op == OPERATOR.Equal)
                return value == key;
            else if (op == OPERATOR.NotEqual)
                return value != key;
            if (value is bool) return false;

            switch (op)
            {
                case OPERATOR.Has:
                    if (value is string)
                        return value.ToString().Contains(key.ToString());
                    else
                        return false;
                case OPERATOR.LessEqual:
                    if (value is string)
                        return (value.ToString()).CompareTo((string)key) <= 0;
                    else if (value is char)
                        return ((char)value).CompareTo((char)key) <= 0;
                    else
                        return Convert.ToDouble(value)<=(Convert.ToDouble(key));
                case OPERATOR.LessThan:
                    if (value is string)
                        return ((string)value).CompareTo((string)key) < 0;
                    else if (value is char)
                        return ((char)value).CompareTo((char)key) < 0;
                    else
                        return Convert.ToDouble(value).CompareTo(Convert.ToDouble(key)) < 0;
                case OPERATOR.MoreEqual:
                    if (value is string)
                        return ((string)value).CompareTo((string)key) >= 0;
                    else if (value is char)
                        return ((char)value).CompareTo((char)key) >= 0;
                    else
                        return Convert.ToDouble(value).CompareTo(Convert.ToDouble(key)) >= 0;
                case OPERATOR.MoreThan:
                    if (value is string)
                        return ((string)value).CompareTo((string)key) > 0;
                    else if (value is char)
                        return ((char)value).CompareTo((char)key) > 0;
                    else
                        return Convert.ToDouble(value).CompareTo(Convert.ToDouble(key)) > 0;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex">点击位置</param>
        /// <param name="features">候选集</param>
        /// <param name="tolerance">点击的搜索范围</param>
        /// <returns></returns>
        public static List<SelectResult> SelectFeaturesByVertex(
            XVertex vertex, 
            List<XFeature> features, 
            double tolerance)
        {
            //初始化返回结果
            List<SelectResult> selection = new List<SelectResult>();
            //定义了一个搜索范围
            XExtent extent = new XExtent(vertex.x - tolerance, vertex.x + tolerance,
                vertex.y - tolerance, vertex.y + tolerance);



            foreach (XFeature feature in features)
            {
                //粗选
                if (!extent.IntersectOrNot(feature.Spatial.extent)) continue;


                //计算距离
                double distance = feature.Spatial.Distance(vertex);


                if (distance <= tolerance)
                    selection.Add(new SelectResult(feature, distance));
            }

            selection.Sort((x, y) => x.criterion.CompareTo(y.criterion));
            return selection;
        }
        
        public static List<SelectResult> SelectFeaturesByExtent(
            XExtent extent, 
            List<XFeature> features)
        {
            List<SelectResult> selection = new List<SelectResult>();
            foreach (XFeature feature in features)
            {
                if (extent.Includes(feature.Spatial.extent))
                    selection.Add(new SelectResult(feature, 0));
            }
            return selection;
        }

        public static List<XFeature> ToFeatures(List<SelectResult> selection)
        {
            List<XFeature> features = new List<XFeature>();
            foreach (SelectResult sr in selection)
                features.Add(sr.feature);
            return features;
        }



    }

    public class XMyFile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct MyFileHeader
        {
            public double MinX, MinY, MaxX, MaxY;
            public int FeatureCount, ShapeType, FieldCount;
        };

        static void WriteFileHeader(XVectorLayer layer, BinaryWriter bw)
        {
            MyFileHeader mfh = new MyFileHeader();
            mfh.MinX = layer.Extent.getMinX();
            mfh.MinY = layer.Extent.getMinY();
            mfh.MaxX = layer.Extent.getMaxX();
            mfh.MaxY = layer.Extent.getMaxY();
            mfh.FeatureCount = layer.FeatureCount();
            mfh.ShapeType = (int)(layer.ShapeType);
            mfh.FieldCount = layer.Fields.Count;
            bw.Write(XTools.FromStructToBytes(mfh));
        }

        public static void WriteFile(XVectorLayer layer, string filename)
        {
            FileStream fsr = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fsr);
            //文件头
            WriteFileHeader(layer, bw);
            //图层名
            XTools.WriteString(layer.Name, bw);
            //字段信息
            WriteFields(layer.Fields, bw);
            //写空间对象
            WriteFeatures(layer, bw);
            //其它内容
            bw.Close();
            fsr.Close();
        }

        static void WriteMultipleVertexes(List<XVertex> vs, BinaryWriter bw)
        {
            bw.Write(vs.Count);
            for (int vc = 0; vc < vs.Count; vc++)
                vs[vc].Write(bw);
        }

        static List<XVertex> ReadMultipleVertexes(BinaryReader br)
        {
            List<XVertex> vs = new List<XVertex>();
            int vcount = br.ReadInt32();
            for (int vc = 0; vc < vcount; vc++)
                vs.Add(new XVertex(br));
            return vs;
        }


        static void WriteFeatures(XVectorLayer layer, BinaryWriter bw)
        {
            for (int featureindex = 0; featureindex < layer.FeatureCount(); featureindex++)
            {
                XFeature feature = layer.GetFeature(featureindex);
                //先写空间部分
                WriteMultipleVertexes(feature.Spatial.vertexes, bw);
                //再写属性部分
                feature.Attribute.Write(bw);
            }
        }

        static void ReadFeatures(
            XVectorLayer layer, 
            BinaryReader br, 
            int FeatureCount)
        {
            for (int featureindex = 0; featureindex < FeatureCount; featureindex++)
            {
                //完成坐标读取
                List<XVertex> vs = ReadMultipleVertexes(br);
                //完成属性数据读取
                XAttribute attribute = new XAttribute(layer.Fields, br);
                //构造空间对象
                XSpatial spatial = null;
                if (layer.ShapeType == SHAPETYPE.Point)
                    spatial = new XPoint(vs[0]);
                else if (layer.ShapeType == SHAPETYPE.Line)
                    spatial = new XLine(vs);
                else if (layer.ShapeType == SHAPETYPE.Polygon)
                    spatial = new XPolygon(vs);
                XFeature feature = new XFeature(spatial, attribute);
                layer.AddFeature(feature);
            }
        }

        public static XVectorLayer ReadFile(string filename)
        {
            //打开文件
            FileStream fsr = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fsr);
            //完成文件头读取
            MyFileHeader mfh = (MyFileHeader)
                (XTools.FromBytes2Struct(br, typeof(MyFileHeader)));
            //获得控件对象类型
            SHAPETYPE ShapeType = 
                (SHAPETYPE)Enum.Parse(typeof(SHAPETYPE), 
                mfh.ShapeType.ToString());
            //读取文件名
            string layername = XTools.ReadString(br);
            //初始化图层
            XVectorLayer layer = new XVectorLayer(layername, ShapeType);
            //赋值图层的空间范围
            layer.Extent = new XExtent(
                mfh.MinX, mfh.MaxX,
                mfh.MinY, mfh.MaxY);
            //读取图层的字段信息
            layer.Fields = ReadFields(br, mfh.FieldCount);
            //读取所有空间对象           
            ReadFeatures(layer, br, mfh.FeatureCount);
            //关闭文件
            br.Close();
            fsr.Close();
            //返回读取到的图层
            return layer;
        }



        static List<Type> AllTypes = new List<Type>{
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(short),
            typeof(string),
            typeof(uint),
            typeof(ulong),
            typeof(ushort)
        };


        static void WriteFields(List<XField> fields, BinaryWriter bw)
        {
            for (int fieldindex = 0; fieldindex < fields.Count; fieldindex++)
            {
                XField field = fields[fieldindex];
                //写入数据类型
                bw.Write(AllTypes.IndexOf(field.datatype));
                //写入字段名称
                XTools.WriteString(field.name, bw);
            }
        }

        static List<XField> ReadFields(BinaryReader br, int FieldCount)
        {
            List<XField> fields = new List<XField>();
            for (int fieldindex = 0; fieldindex < FieldCount; fieldindex++)
            {
                Type fieldtype = AllTypes[br.ReadInt32()];
                string fieldname = XTools.ReadString(br);
                fields.Add(new XField(fieldtype, fieldname));
            }
            return fields;
        }



    }



    public class XTools
    {
        /// <summary>
        /// 计算C到线段AB的距离
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static double DistanceBetweenPointAndSegment(
            XVertex A, XVertex B, XVertex C)
        {
            if (A.IsSame(B)) return B.Distance(C);
            double dot1 = DotProduct(A, B, C);
            if (dot1 > 0) return B.Distance(C);
            double dot2 = DotProduct(B, A, C);
            if (dot2 > 0) return A.Distance(C);
            double dist = CrossProduct(A, B, C) / A.Distance(B);
            return Math.Abs(dist);
        }
        /// <summary>
        /// AB与BC的点积
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        static double DotProduct(XVertex A, XVertex B, XVertex C)
        {
            XVertex AB = new XVertex(B.x - A.x, B.y - A.y);
            XVertex BC = new XVertex(C.x - B.x, C.y - B.y);
            return AB.x * BC.x + AB.y * BC.y;
        }

        /// <summary>
        /// AB与AC的叉积
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        static double CrossProduct(XVertex A, XVertex B, XVertex C)
        {
            XVertex AB = new XVertex(B.x - A.x, B.y - A.y);
            XVertex AC = new XVertex(C.x - A.x, C.y - A.y);
            return VectorProduct(AB, AC);
        }





        public static void WriteString(string s, BinaryWriter bw)
        {
            byte[] sbytes = Encoding.GetEncoding("gb2312").GetBytes(s);
            bw.Write(sbytes.Length);
            bw.Write(sbytes);
        }

        public static string ReadString(BinaryReader br)
        {
            int length = br.ReadInt32();
            byte[] sbytes = br.ReadBytes(length);
            return Encoding.GetEncoding("gb2312").GetString(sbytes);
        }


        public static string BytesToString(byte[] byteArray)
        {
            int startIndexOfZero = byteArray.Length;
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (byteArray[i] == 0)
                {
                    startIndexOfZero = i;
                    break;
                }
            }
            return Encoding.
                GetEncoding("gb2312").
                GetString(byteArray, 0, startIndexOfZero);
        }

        public static double CalculateLength(List<XVertex> _vertexes)
        {
            double length = 0;
            for (int i = 0; i < _vertexes.Count - 1; i++)
            {
                length += _vertexes[i].Distance(_vertexes[i + 1]);
            }
            return length;
        }

        public static double CalculateArea(List<XVertex> _vertexes)
        {
            double area = 0;
            for (int i = 0; i < _vertexes.Count - 1; i++)
            {
                area += VectorProduct(_vertexes[i], _vertexes[i + 1]);
            }
            area += VectorProduct(_vertexes[_vertexes.Count - 1], _vertexes[0]);
            return area / 2;
        }

        public static double VectorProduct(XVertex v1, XVertex v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }


        public static int ReverseInt(int value)
        {
            byte[] barray = BitConverter.GetBytes(value);
            Array.Reverse(barray);
            return BitConverter.ToInt32(barray, 0);
        }

        public static Object FromBytes2Struct(BinaryReader br, Type type)
        {
            byte[] buff = br.ReadBytes(Marshal.SizeOf(type));
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            Object result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            handle.Free();
            return result;
        }

        public static byte[] FromStructToBytes(object struc)
        {
            byte[] bytes = new byte[Marshal.SizeOf(struc.GetType())];
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(struc, handle.AddrOfPinnedObject(), false);
            handle.Free();
            return bytes;
        }

    }

    public class XShapefile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DBFField
        {
            public byte b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11;
            public byte FieldType;
            public int DisplacementInRecord;
            public byte LengthOfField;
            public byte NumberOfDecimalPlaces;
            public long Unused1;
            public int Unused2;
            public short Unused3;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct DBFHeader
        {
            public byte FileType, Year, Month, Day;
            public int RecordCount;
            public short HeaderLength, RecordLength;
            public long Unused1, Unused2;
            public int Unused3;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct ShapefileHeader
        {
            public double Unused1, Unused2, Unused3, Unused4;
            public int ShapeType;
            public double Xmin;
            public double Ymin;
            public double Xmax;
            public double Ymax;
            public double Unused9, Unused10, Unused11, Unused12;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct RecordHeader
        {
            public int RecordNumber;
            public int RecordLength;
            public int ShapeType;
        };

        static List<XAttribute> ReadDBFValues(
            string dbffilename, List<XField> fields)
        {
            FileStream fsr = new FileStream(dbffilename, FileMode.Open);
            BinaryReader br = new BinaryReader(fsr);

            DBFHeader dh = (DBFHeader)XTools.FromBytes2Struct(
                br, typeof(DBFHeader));
            int FieldCount = (dh.HeaderLength - 33) / 32;

            br.ReadBytes(32 * FieldCount + 1); //跳过字段区及结束标志字节

            //开始读数据
            List<XAttribute> attributes = new List<XAttribute>();
            for (int i = 0; i < dh.RecordCount; i++) //开始读取具体数值
            {
                XAttribute attribute = new XAttribute();

                char tempchar = (char)br.ReadByte();  
                //每个记录的开始都有一个起始字节

                for (int j = 0; j < FieldCount; j++)
                    attribute.AddValue(
                        fields[j].DBFValueToObject(br)
                        );
                attributes.Add(attribute);
            }
            br.Close();
            fsr.Close();
            return attributes;
        }

        static List<XField> ReadDBFFields(string dbffilename)
        {
            FileStream fsr = new FileStream(dbffilename, FileMode.Open);
            BinaryReader br = new BinaryReader(fsr);

            DBFHeader dh = (DBFHeader)
                XTools.FromBytes2Struct(
                    br, typeof(DBFHeader));


            int FieldCount = (dh.HeaderLength -32-1) / 32;


            List<XField> fields = new List<XField>();

            for (int i = 0; i < FieldCount; i++)
                fields.Add(new XField(br));

            br.Close();
            fsr.Close();
            return fields;
        }

        static ShapefileHeader ReadFileHeader(BinaryReader br)
        {
            return (ShapefileHeader)XTools.FromBytes2Struct(br, typeof(ShapefileHeader));
        }

        static RecordHeader ReadRecordHeader(BinaryReader br)
        {
            return (RecordHeader)XTools.FromBytes2Struct(br, typeof(RecordHeader));
        }


        public static XVectorLayer ReadShapefile(string shpfilename)
        {
            try
            {
                FileStream fsr = new FileStream(shpfilename, FileMode.Open);
                BinaryReader br = new BinaryReader(fsr);


                ShapefileHeader sfh = ReadFileHeader(br);


                SHAPETYPE ShapeType = Int2Shapetype[sfh.ShapeType];


                XVectorLayer layer = new XVectorLayer(shpfilename, ShapeType);



                layer.Extent = new XExtent(sfh.Xmax, sfh.Xmin, sfh.Ymax, sfh.Ymin);


                string dbffilename = shpfilename.ToLower().Replace(".shp", ".dbf");
                layer.Fields = ReadDBFFields(dbffilename);
                List<XAttribute> attributes = ReadDBFValues(dbffilename, layer.Fields);


                //其他代码
                int index = 0;
                while (br.PeekChar() != -1)
                {
                    RecordHeader rh = ReadRecordHeader(br);
                    int ByteLength = XTools.ReverseInt(rh.RecordLength) * 2 - 4;
                    byte[] RecordContent = br.ReadBytes(ByteLength);
                    if (ShapeType == SHAPETYPE.Point)
                    {
  
                        XPoint onepoint = ReadPoint(RecordContent);
                        XFeature feature = new XFeature(onepoint, attributes[index]);
                        layer.AddFeature(feature);
                    }
                    else if (ShapeType == SHAPETYPE.Line)
                    {
                        List<XLine> lines = ReadLines(RecordContent);
                        foreach (XLine line in lines)
                        {
                            XFeature feature = new XFeature(line, new XAttribute(attributes[index]));
                            layer.AddFeature(feature);
                        }
                    }
                    else if (ShapeType == SHAPETYPE.Polygon)
                    {
                        List<XPolygon> polygons = ReadPolygons(RecordContent);
                        foreach (XPolygon polygon in polygons)
                        {
                            var b = Guid.NewGuid().ToByteArray();

                            XFeature feature = new XFeature(polygon, new XAttribute(attributes[index]));
                            layer.AddFeature(feature);
                        }
                    }
                    index++;
                    //其他代码
                }




                br.Close();
                fsr.Close();
                return layer;
            }
            catch (Exception ex)
            {

               
            }

            return new XVectorLayer("",  SHAPETYPE.unknown); ;
        }
        static List<XPolygon> ReadPolygons(byte[] RecordContent)
        {
            int N = BitConverter.ToInt32(RecordContent, 32);
            int M = BitConverter.ToInt32(RecordContent, 36);
            int[] parts = new int[N + 1];
            for (int i = 0; i < N; i++)
            {
                parts[i] = BitConverter.ToInt32(RecordContent, 40 + i * 4);
            }
            parts[N] = M;

            List<XPolygon> polygons = new List<XPolygon>();
            for (int i = 0; i < N; i++)
            {
                List<XVertex> vertexes = new List<XVertex>();
                for (int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16);
                    double y = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16 + 8);
                    vertexes.Add(new XVertex(x, y));
                }
                polygons.Add(new XPolygon(vertexes));
            }
            return polygons;
        }

        static List<XLine> ReadLines(byte[] RecordContent)
        {
            int N = BitConverter.ToInt32(RecordContent, 32);
            int M = BitConverter.ToInt32(RecordContent, 36);
            int[] parts = new int[N + 1];

            for (int i = 0; i < N; i++)
            {
                parts[i] = BitConverter.ToInt32(RecordContent, 40 + i * 4);
            }
            parts[N] = M;
            List<XLine> lines = new List<XLine>();
            for (int i = 0; i < N; i++)
            {
                List<XVertex> vertexes = new List<XVertex>();
                for (int j = parts[i]; j < parts[i + 1]; j++)
                {
                    double x = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16);
                    double y = BitConverter.ToDouble(RecordContent, 40 + N * 4 + j * 16 + 8);
                    vertexes.Add(new XVertex(x, y));
                }
                lines.Add(new XLine(vertexes));
            }
            return lines;
        }

        /// <summary>
        /// read one point from the input byte array
        /// the length of this array is 16, including 2 double values, i.e., x and y
        /// </summary>
        /// <param name="recordContent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static XPoint ReadPoint(byte[] RecordContent)
        {
            double x = BitConverter.ToDouble(RecordContent, 0);
            double y = BitConverter.ToDouble(RecordContent, 8);
            return new XPoint(new XVertex(x, y));
        }

        static Dictionary<int, SHAPETYPE> Int2Shapetype = new Dictionary<int, SHAPETYPE>
        {
            {1, SHAPETYPE.Point },
            {3, SHAPETYPE.Line },
            {5, SHAPETYPE.Polygon }
        };


    }

    public class XVectorLayer
    {
        public string Name;  //图层名
        public SHAPETYPE ShapeType;  //控件对象类型
        public List<XFeature> Features = new List<XFeature>();
        public XExtent Extent;
        public List<XField> Fields = new List<XField>();
        public bool LabelOrNot = true;
        public int LabelIndex = 0;
        public Color labelColor= Color.Gray;

        public List<XFeature> SelectedFeatures = new List<XFeature>();
        public XThematic UnselectedThematic, SelectedThematic;



        public XVectorLayer(string _name, SHAPETYPE _shapetype)
        {
            Name = _name;
            ShapeType = _shapetype;

            UnselectedThematic = new XThematic();

            SelectedThematic = new XThematic(
                new Pen(Color.Red, 1),
                new Pen(Color.Red, 1), new SolidBrush(Color.Pink),
                new Pen(Color.Red, 1), new SolidBrush(Color.Pink), 5);

        }

        public void SelectByAttribute(
            XSelect.OPERATOR op, 
            int fieldIndex, 
            object key, 
            bool modify)
        {
            List<XFeature> features = XSelect.SelectFeaturesByAttribute(
                Features, op, fieldIndex, key);
            ModifySelection(features, modify);
        }

        public void SelectByVertex(XVertex vertex, double tolerance, bool modify)
        {
            List<XFeature> features = XSelect.ToFeatures(
                XSelect.SelectFeaturesByVertex(vertex, Features, tolerance));
            ModifySelection(features, modify);
        }

        public void SelectByExtent(XExtent extent, bool modify)
        {
            List<XFeature> features = XSelect.ToFeatures(
                XSelect.SelectFeaturesByExtent(extent, Features));
            ModifySelection(features, modify);
        }

        private void ModifySelection(List<XFeature> features, bool modify)
        {
            if (!modify)
            {
                SelectedFeatures = features;
            }
            else
            {
                bool IncludeAll = true;
                foreach (XFeature feature in features)
                {
                    if (!SelectedFeatures.Contains(feature))
                    {
                        //情景2：添加入选择集
                        IncludeAll = false;
                        SelectedFeatures.Add(feature);
                    }
                }
                if (IncludeAll)
                {
                    //情景1：从选择集中移出
                    foreach (XFeature feature in features)
                    {
                        SelectedFeatures.Remove(feature);
                    }
                }
            }
        }


        public void UpdateExtent()
        {
            if (Features.Count == 0)
                Extent = null;
            else
            {
                //Extent = Features[0].Spatial.extent;
                Extent = new XExtent(Features[0].Spatial.extent);
                for (int i = 1; i < Features.Count; i++)
                    Extent.Merge(Features[i].Spatial.extent);
            }
        }

        public void AddFeature(XFeature feature)
        {
            Features.Add(feature);
            if (Features.Count == 1)
                Extent = new XExtent(feature.Spatial.extent);
            else
                Extent.Merge(feature.Spatial.extent);
        }

        public void RemoveFeature(int index)
        {
            Features.RemoveAt(index);
            UpdateExtent();
        }


        public int FeatureCount()
        {
            return Features.Count;
        }

        public XFeature GetFeature(int index)
        {
            return Features[index];
        }

        public void Clear()
        {
            Features.Clear();
            Extent = null;
        }

        public void draw(Graphics graphics, XView view)
        {
            if (Extent == null) return;
            if (!Extent.IntersectOrNot(view.CurrentMapExtent)) return;
            for (int i = 0; i < Features.Count; i++)
            {
       
                if (Features[i].Spatial.extent.IntersectOrNot(view.CurrentMapExtent))
                    Features[i].draw(graphics, view,
                         Form2.isShowLabel, LabelIndex, labelColor,
                        SelectedFeatures.Contains(Features[i]) ?
                                SelectedThematic 
                                :
                                UnselectedThematic
                    );
            }
        }

        internal void DeleteSelected()
        {
            foreach(XFeature feature in SelectedFeatures)
            {
                Features.Remove(feature);
            }
            SelectedFeatures.Clear();
            UpdateExtent();
        }

        internal void SaveSelected(string path)
        {
            XVectorLayer newLayer = new XVectorLayer(Name, ShapeType);
            newLayer.Extent = Extent;
            newLayer.Fields = Fields;
            newLayer.Features = SelectedFeatures;
            XMyFile.WriteFile(newLayer, path);
        }
    }
    public enum SHAPETYPE
    {
        Point,
        Line,
        Polygon,
        unknown
    };



    public class XField
    {
        public Type datatype;
        public string name;
        public int DBFFieldLength;

        public XField(Type _datatype, string _name)
        {
            datatype = _datatype;
            name = _name;
        }



        public XField(BinaryReader br)
        {
            XShapefile.DBFField dbfField =
                (XShapefile.DBFField)XTools.FromBytes2Struct(
                    br, typeof(XShapefile.DBFField));

            DBFFieldLength = dbfField.LengthOfField;


            byte[] bs = new byte[] {dbfField.b1,
                dbfField.b2,dbfField.b3, dbfField.b4,dbfField.b5,
                dbfField.b6,dbfField.b7,dbfField.b8,dbfField.b9,
                dbfField.b10,dbfField.b11};
            
            
            name = XTools.BytesToString(bs);



            switch ((char)dbfField.FieldType)
            {
                case 'N':
                    if (dbfField.NumberOfDecimalPlaces == 0)
                        datatype = Type.GetType("System.Int32");
                    else
                        datatype = Type.GetType("System.Double");
                    break;
                case 'F':
                    datatype = Type.GetType("System.Double");
                    break;
                default:
                    datatype = Type.GetType("System.String");
                    break;
            }
        }

        public object DBFValueToObject(BinaryReader br)
        {
            byte[] temp = br.ReadBytes(DBFFieldLength);
            string sv = XTools.BytesToString(temp);
            if (datatype == Type.GetType("System.String"))
                return sv;
            else if (datatype == Type.GetType("System.Double"))
                return double.Parse(sv);
            else if (datatype == Type.GetType("System.Int32"))
            {
                 int.TryParse(sv, out int result);
                return result;
            }
            else
                return sv;
        }

    }

    public enum XExploreActions
    {
        noaction,
        zoomin, zoomout,
        moveup, movedown, moveleft, moveright,
        pan,
        zoominbybox,
        select
    };

    public class XView
    {
        public XExtent CurrentMapExtent;
        Rectangle MapWindowSize;
        double MapMinX, MapMinY;
        int WinW, WinH;
        double MapW, MapH;
        public double ScaleX, ScaleY;

        public XView(XExtent _Extent, Rectangle _Rectangle)
        {
            Update(_Extent, _Rectangle);
        }
        public void Update(XExtent _extent, Rectangle _rectangle)
        {
            //给地图窗口赋值
            MapWindowSize = _rectangle;
            //计算地图窗口的宽度
            WinW = MapWindowSize.Width;
            //计算地图窗口的高度
            WinH = MapWindowSize.Height;
            //计算比例尺
            ScaleX = ScaleY = Math.Max(_extent.getWidth() / WinW, _extent.getHeight() / WinH);
            //根据比例尺计算新的地图范围的宽度
            MapW = ScaleX * WinW;
            //根据比例尺计算新的地图范围的高度
            MapH = ScaleY * WinH;
            //获得地图范围中心
            XVertex center = _extent.getCenter();
            //根据地图范围的中心，计算最小坐标极值
            MapMinX = center.x - MapW / 2;
            MapMinY = center.y - MapH / 2;
            //计算当前显示的实际地图范围
            CurrentMapExtent = new XExtent(
                new XVertex(MapMinX, MapMinY),
                new XVertex(MapMinX + MapW, MapMinY + MapH));
                
                
                
                //new XExtent(MapMinX, MapMinX + MapW, MapMinY, MapMinY + MapH);
        }

        public void Update_old(XExtent _Extent, Rectangle _Rectangle)
        {
            CurrentMapExtent = _Extent;
            MapWindowSize = _Rectangle;
            MapMinX =  CurrentMapExtent.getMinX();
            MapMinY = CurrentMapExtent.getMinY();
            WinW = MapWindowSize.Width;
            WinH = MapWindowSize.Height;
            MapW = CurrentMapExtent.getWidth();
            MapH = CurrentMapExtent.getHeight();
            ScaleX = MapW / WinW;
            ScaleY = MapH / WinH;
            ScaleX = ScaleY=Math.Max(ScaleX, ScaleY);
        }

        public Point ToScreenPoint(XVertex onevertex)
        {
            double ScreenX = (onevertex.x - MapMinX) / ScaleX;
            double ScreenY = WinH - (onevertex.y - MapMinY) / ScaleY;
            return new Point((int)ScreenX, (int)ScreenY);
        }

        public XVertex ToMapVertex(Point point)
        {
            double MapX = ScaleX * point.X + MapMinX;
            double MapY = ScaleY * (WinH - point.Y) + MapMinY;
            return new XVertex(MapX, MapY);
        }

        public double ToScreenDistance(double mapDistance)
        {
            XVertex vertex=CurrentMapExtent.bottomleft;
            Point p1 = ToScreenPoint(vertex);
            Point p2 = ToScreenPoint(new XVertex(vertex.x - mapDistance, vertex.y));
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        internal void ChangeView(XExploreActions action)
        {
            CurrentMapExtent.ChangeExtent(action);
            Update(CurrentMapExtent, MapWindowSize);
        }

        internal void UpdateMapWindow(Rectangle clientRectangle)
        {
            Update(CurrentMapExtent, clientRectangle);
        }

        internal void OffsetCenter(XVertex vFrom, XVertex vTo)
        {
            Point pFrom = ToScreenPoint(vFrom);
            Point pTo = ToScreenPoint(vTo);


            Point newScreenCenter =  new Point(
                MapWindowSize.Width / 2 - (pTo.X - pFrom.X),
                MapWindowSize.Height / 2 - pTo.Y + pFrom.Y);

            XVertex newMapCenter = ToMapVertex(newScreenCenter);

            UpdateMapCenter(newMapCenter);

        }

        public void UpdateMapCenter(XVertex newCenter)
        {
            CurrentMapExtent.SetCenter(newCenter);
            Update(CurrentMapExtent, MapWindowSize);
        }

        internal List<Point> ToScreenPoints(List<XVertex> vertexes)
        {
            List<Point> points=new List<Point>();
            foreach(XVertex v in vertexes)
            {
                points.Add(ToScreenPoint(v));
            }
            return points;
        }

        public double ToMapDistance(int pixelCount)
        {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, pixelCount);
            XVertex v1 = ToMapVertex(p1);
            XVertex v2 = ToMapVertex(p2);
            return v1.Distance(v2);
        }

    }

    public class XExtent
    {
        public XVertex bottomleft;
        public XVertex upright;

        public XExtent(double x1, double x2, double y1, double y2)
        {
            upright = new XVertex(Math.Max(x1, x2),
                                Math.Max(y1, y2));
            bottomleft = new XVertex(Math.Min(x1, x2),
                                Math.Min(y1, y2));
        }

        public XExtent(XVertex _oneCorner, XVertex _anotherCorner)
        {
            upright = new XVertex(Math.Max(_anotherCorner.x, _oneCorner.x),
                                Math.Max(_anotherCorner.y, _oneCorner.y));
            bottomleft = new XVertex(Math.Min(_anotherCorner.x, _oneCorner.x),
                                Math.Min(_anotherCorner.y, _oneCorner.y));
        }

        //public XExtent(XVertex _bottomLeft, XVertex _upRight)
        //{
        //    bottomleft = _bottomLeft;
        //    upright = _upRight;
        //}

        public XExtent(XExtent extent)
        {
            bottomleft =new XVertex(extent.bottomleft);
            upright = new XVertex(extent.upright);
        }

        public double getMinX()
        {
            return bottomleft.x;
        }

        public double getMaxX()
        {
            return upright.x;
        }

        public double getMinY()
        {
            return bottomleft.y;
        }

        public double getMaxY()
        {
            return upright.y;
        }

        public double getWidth()
        {
            return upright.x - bottomleft.x;
        }

        public double getHeight()
        {
            return upright.y - bottomleft.y;
        }

        /// <summary>
        /// 根据不通过的action来修改角点
        /// </summary>
        /// <param name="action"></param>
        internal void ChangeExtent(XExploreActions action)
        {
            double new_minx = getMinX();
            double new_miny = getMinY();
            double new_maxx = getMaxX();
            double new_maxy = getMaxY();

            double move_factor = getWidth() * 0.25;
            double ZoomFactor = 1.1;

            switch (action)
            {
                case XExploreActions.moveup:
                    new_miny -= move_factor;
                    new_maxy -= move_factor;
                    break;
                case XExploreActions.movedown:
                    new_miny += move_factor;
                    new_maxy += move_factor;
                    break;
                case XExploreActions.moveleft:
                    new_minx += move_factor;
                    new_maxx += move_factor;
                    break;
                case XExploreActions.moveright:
                    new_minx -= move_factor;
                    new_maxx -= move_factor;
                    break;
                case XExploreActions.zoomin:
                    new_minx = ((getMinX() + getMaxX()) - getWidth() / ZoomFactor) / 2;
                    new_miny = ((getMinY() + getMaxY()) - getHeight() / ZoomFactor) / 2;
                    new_maxx = ((getMinX() + getMaxX()) + getWidth() / ZoomFactor) / 2;
                    new_maxy = ((getMinY() + getMaxY()) + getHeight() / ZoomFactor) / 2;
                    break;
                case XExploreActions.zoomout:
                    new_minx = ((getMinX() + getMaxX()) - getWidth() * ZoomFactor) / 2;
                    new_miny = ((getMinY() + getMaxY()) - getHeight() * ZoomFactor) / 2;
                    new_maxx = ((getMinX() + getMaxX()) + getWidth() * ZoomFactor) / 2;
                    new_maxy = ((getMinY() + getMaxY()) + getHeight() * ZoomFactor) / 2;
                    break;
            }
            bottomleft = new XVertex(new_minx, new_miny);
            upright=new XVertex(new_maxx, new_maxy);
        }

        internal XVertex getCenter()
        {
            return new XVertex((upright.x + bottomleft.x) / 2, (upright.y + bottomleft.y) / 2);
        }

        public bool IntersectOrNot(XExtent extent)
        {
            return !(getMaxX() < extent.getMinX() || getMinX() > extent.getMaxX()
                || getMaxY() < extent.getMinY() || getMinY() > extent.getMaxY());
        }

        internal void Merge(XExtent extent)
        {
            bottomleft.x=Math.Min(getMinX(), extent.getMinX());
            bottomleft.y = Math.Min(getMinY(), extent.getMinY());

            upright.x = Math.Max(getMaxX(), extent.getMaxX());
            upright.y = Math.Max(getMaxY(), extent.getMaxY());
        }

        internal void SetCenter(XVertex newCenter)
        {
            double width = getWidth();
            double height = getHeight();
            upright = new XVertex(newCenter.x + width / 2, newCenter.y + height / 2);
            bottomleft = new XVertex(newCenter.x - width / 2, newCenter.y - height / 2);
        }

        public bool Includes(XExtent extent)
        {
            return (
                getMaxX() >= extent.getMaxX() &&
                getMinX() <= extent.getMinX() &&
                getMaxY() >= extent.getMaxY() &&
                getMinY() <= extent.getMinY());
        }

    }

    public abstract class XSpatial
    {
        public XVertex centroid;
        public XExtent extent;
        public List<XVertex> vertexes;

        public XSpatial(List<XVertex> _vertexes)
        {
            //为节点数组赋值
            vertexes = _vertexes;

            //计算中心点centroid
            double x_cen = 0, y_cen = 0;
            foreach (XVertex v in _vertexes)
            {
                x_cen += v.x;
                y_cen += v.y;
            }
            x_cen /= _vertexes.Count;
            y_cen /= _vertexes.Count;
            centroid = new XVertex(x_cen, y_cen);

            //计算空间范围extent
            double x_min = double.MaxValue;
            double y_min = double.MaxValue;
            double x_max = double.MinValue;
            double y_max = double.MinValue;

            foreach (XVertex v in _vertexes)
            {
                x_min = Math.Min(x_min, v.x);
                y_min = Math.Min(y_min, v.y);
                x_max = Math.Max(x_max, v.x);
                y_max = Math.Max(y_max, v.y);
            }
            extent = new XExtent(new XVertex(x_min, y_min),
                new XVertex(x_max, y_max));
        }
        public abstract void Draw(Graphics graphics, XView view,
            XThematic thematic,int type=0);

        public abstract double Distance(XVertex v);

    }

    public class XAttribute
    {
        ArrayList Attributes = new ArrayList();
        public int type = 0;
        public XAttribute()
        {
        }

        public XAttribute(XAttribute a, int type=0)
        {
            foreach (object v in a.Attributes)
                Attributes.Add(v);
            this.type = type;
        }

        public void Write(BinaryWriter bw)
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                Type type = GetValue(i).GetType();
                if (type.ToString() == "System.Boolean")
                    bw.Write((bool)GetValue(i));
                else if (type.ToString() == "System.Byte")
                    bw.Write((byte)GetValue(i));
                else if (type.ToString() == "System.Char")
                    bw.Write((char)GetValue(i));
                else if (type.ToString() == "System.Decimal")
                    bw.Write((decimal)GetValue(i));
                else if (type.ToString() == "System.Double")
                    bw.Write((double)GetValue(i));
                else if (type.ToString() == "System.Single")
                    bw.Write((float)GetValue(i));
                else if (type.ToString() == "System.Int32")
                    bw.Write((int)GetValue(i));
                else if (type.ToString() == "System.Int64")
                    bw.Write((long)GetValue(i));
                else if (type.ToString() == "System.UInt16")
                    bw.Write((ushort)GetValue(i));
                else if (type.ToString() == "System.UInt32")
                    bw.Write((uint)GetValue(i));
                else if (type.ToString() == "System.UInt64")
                    bw.Write((ulong)GetValue(i));
                else if (type.ToString() == "System.SByte")
                    bw.Write((sbyte)GetValue(i));
                else if (type.ToString() == "System.Int16")
                    bw.Write((short)GetValue(i));
                else if (type.ToString() == "System.String")
                    XTools.WriteString((string)GetValue(i), bw);
            }
        }

        public XAttribute(List<XField> fs, BinaryReader br)
        {
            for (int i = 0; i < fs.Count; i++)
            {
                Type type = fs[i].datatype;
                if (type.ToString() == "System.Boolean")
                    AddValue(br.ReadBoolean());
                else if (type.ToString() == "System.Byte")
                    AddValue(br.ReadByte());
                else if (type.ToString() == "System.Char")
                    AddValue(br.ReadChar());
                else if (type.ToString() == "System.Decimal")
                    AddValue(br.ReadDecimal());
                else if (type.ToString() == "System.Double")
                    AddValue(br.ReadDouble());
                else if (type.ToString() == "System.Single")
                    AddValue(br.ReadSingle());
                else if (type.ToString() == "System.Int32")
                    AddValue(br.ReadInt32());
                else if (type.ToString() == "System.Int64")
                    AddValue(br.ReadInt64());
                else if (type.ToString() == "System.UInt16")
                    AddValue(br.ReadUInt16());
                else if (type.ToString() == "System.UInt32")
                    AddValue(br.ReadUInt32());
                else if (type.ToString() == "System.UInt64")
                    AddValue(br.ReadUInt64());
                else if (type.ToString() == "System.SByte")
                    AddValue(br.ReadSByte());
                else if (type.ToString() == "System.Int16")
                    AddValue(br.ReadInt16());
                else if (type.ToString() == "System.String")
                    AddValue(XTools.ReadString(br));
            }
        }



        public void AddValue(object value)
        {
            Attributes.Add(value);
        }

        public object GetValue(int index)
        {
            return Attributes[index];
        }

        public void Draw(Graphics graphics,XView view, int index, XVertex Location, Color color)
        {
      
            Point point=view.ToScreenPoint(Location);
            graphics.DrawString(
                GetValue(index).ToString(),
                new Font("宋体", 10),
                new SolidBrush(color), point);
  
        }
    }

    public class XFeature
    {
        public XSpatial Spatial;
        public XAttribute Attribute;

        public XFeature(XSpatial _spatial, XAttribute _attribute)
        {
            Spatial = _spatial;
            Attribute = _attribute;
        }

        public void draw(Graphics graphics, XView view, 
            bool DrawAttributeOrNot, int attributeIndex, Color labelColor, 
            XThematic thematic)
        {
            int.TryParse((GetAttribute(1) ?? "0").ToString(), out int value);

            if (value > 0 && value < 15)
            {
                Attribute.type = 1;
            }
            else if (value >=15 && value < 47)
            {
                Attribute.type = 2;
            }
            else if (value >=47 && value < 73)
            {
                Attribute.type = 3;
            }
            else if (value >=73 && value < 106)
            {
                Attribute.type = 4;
            }
            else if (value >=106)
            {
                Attribute.type = 5;
            }
            Spatial.Draw(graphics, view, thematic,Attribute.type);
            if (DrawAttributeOrNot)
            {
                if ( !(Spatial is XPoint))
                    Attribute.Draw(graphics, view, attributeIndex, Spatial.centroid, labelColor);
            }
        }

        public object GetAttribute(int index)
        {
            return Attribute.GetValue(index);
        }

        public double Distance(XVertex v)
        {
            return Spatial.Distance(v);
        }
    }

    public class XVertex
    {
        public double x;
        public double y;

        public XVertex(XVertex v)
        {
            x= v.x;
            y= v.y;
        }

        public XVertex(double _x, double _y)
        {
            this.x = _x;
            this.y = _y;
        }

        internal double Distance(XVertex v)
        {
            return Math.Sqrt((x - v.x) * (x - v.x) + (y - v.y) * (y - v.y));
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(x);
            bw.Write(y);
        }

        internal bool IsSame(XVertex b)
        {
            return x==b.x && y==b.y;
        }

        public XVertex(BinaryReader br)
        {
            x = br.ReadDouble();
            y = br.ReadDouble();
        }

    }

    public class XPoint: XSpatial
    {
        public XPoint(XVertex onevertex) 
            : base(new List<XVertex>(){ onevertex })
        {
        }

        public override double Distance(XVertex anothervertex)
        {
            return centroid.Distance(anothervertex);
        }

        public override void Draw(Graphics graphics, XView view,
            XThematic thematic,int type=0)
        {
            
            Point point = view.ToScreenPoint(centroid);
            if(Form1.isCheckTyphoon)
            {
                int s = (int)thematic.PointRadius * (300/20);
                graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, 102, 153, 255)),
                  new Rectangle(
                      point.X - s,
                      point.Y - s,
                      s * 2,
                      s * 2));
                graphics.DrawEllipse(thematic.PointPen,
                   new Rectangle(
                       point.X - s,
                       point.Y - s,
                       s * 2,
                       s * 2));
            }
         
            graphics.FillEllipse(thematic.PointBrush,
                new Rectangle(
                    point.X - thematic.PointRadius, 
                    point.Y - thematic.PointRadius, 
                    thematic.PointRadius*2, 
                    thematic.PointRadius*2));
            graphics.DrawEllipse(thematic.PointPen,
                new Rectangle(
                    point.X - thematic.PointRadius,
                    point.Y - thematic.PointRadius,
                    thematic.PointRadius * 2,
                    thematic.PointRadius * 2));
            if(Form1.isCheckTyphoon)
            {
                graphics.DrawEllipse(thematic.PointPen,
            new Rectangle(
                point.X - thematic.PointRadius,
                point.Y - thematic.PointRadius,
                thematic.PointRadius * 2,
                thematic.PointRadius * 2));
            }
        }
    }

    public class XLine: XSpatial
    {
        public double length;

        public XLine(List<XVertex> vertices) : base(vertices)
        {
            length=XTools.CalculateLength(vertices);
        }

        public override void Draw(Graphics graphics, XView view, XThematic thematic, int type = 0)
        {
            List<Point> points = view.ToScreenPoints(vertexes);
            graphics.DrawLines(thematic.LinePen, points.ToArray());
        }

        public override double Distance(XVertex vertex)
        {
            double distance = Double.MaxValue;
            for (int i = 0; i < vertexes.Count - 1; i++)
            {
                distance = Math.Min(
                    XTools.DistanceBetweenPointAndSegment(
                    vertexes[i], vertexes[i + 1], vertex),
                    distance);
            }
            return distance;

        }
    }

    public class XPolygon : XSpatial
    {
        public double area;
        public XPolygon(List<XVertex> vertices) : base(vertices)
        {
            area = XTools.CalculateArea(vertices);
        }

        /// <summary>
        /// 判断一个点与一个多边形之间的拓扑关系
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="inside">如果是在内部，则为true，
        /// 如果是在边线上，则为false</param>
        /// <returns>如果点在多边形的内部或者边线上，则为true
        /// 否则为false</returns>
        public bool Contains(XVertex vertex, out bool inside)
        {
            int count = 0;
            inside = true;
            for (int i = 0; i < vertexes.Count; i++)
            {
                //满足情况3
                if (vertexes[i].IsSame(vertex))
                {
                    inside = false;
                    return true;
                }
                //由序号为i及next的两个节点构成一条线段，
                //一般情况下next为i+1，
                //而针对最后一条线段，i为vertexes.Count-1，next为0
                int next = (i + 1) % vertexes.Count;
                //确定线段的坐标极值
                double minX = Math.Min(vertexes[i].x, vertexes[next].x);
                double minY = Math.Min(vertexes[i].y, vertexes[next].y);
                double maxX = Math.Max(vertexes[i].x, vertexes[next].x);
                double maxY = Math.Max(vertexes[i].y, vertexes[next].y);
                //如果线段是平行于射线的。
                if (minY == maxY)
                {
                    //满足情况2
                    if (minY == vertex.y && vertex.x >= minX && vertex.x <= maxX)
                    {
                        inside = false;
                        return true;
                    }
                    //满足情况1或者射线与线段平行无交点
                    else continue;
                }
                //点在线段坐标极值之外，不可能有交点
                if (vertex.x > maxX || vertex.y > maxY || vertex.y < minY) continue;
                //计算交点横坐标，纵坐标无需计算，就是vertex.y
                double X0 = vertexes[i].x + (vertex.y - vertexes[i].y) *
                    (vertexes[next].x - vertexes[i].x) / (vertexes[next].y - vertexes[i].y);
                //交点在射线反方向，按无交点计算
                if (X0 < vertex.x) continue;
                //交点即为vertex，且在线段上
                if (X0 == vertex.x)
                {
                    inside = false;
                    return true;
                }
                //射线穿过线段下端点，不记数
                if (vertex.y == minY)
                    continue;
                //其他情况下，交点数加一
                count++;
            }
            //根据交点数量确定面是否包括点
            return count % 2 != 0;
        }

        public override double Distance(XVertex vertex)
        {
            bool inside;
            if (Contains(vertex, out inside))
            {
                if (inside) return -1;
                else return 0;
            }
            else
            {
                List<XVertex> vs = new List<XVertex>();
                vs.AddRange(vertexes);
                vs.Add(vertexes[0]);
                XLine line = new XLine(vs);
                return line.Distance(vertex);
            }

        }

        public override void Draw(Graphics graphics, XView view, XThematic thematic,int type=0)
        {
            Point[] points = view.ToScreenPoints(vertexes).ToArray();
            if(Form1.isCheckMap)
            {
                switch (type)
                {
                    case 1:
                        graphics.FillPolygon(thematic.PolygonBrush1, points);
                        break;
                    case 2:
                        graphics.FillPolygon(thematic.PolygonBrush2, points);
                        break;
                    case 3:
                        graphics.FillPolygon(thematic.PolygonBrush3, points);
                        break;
                    case 4:
                        graphics.FillPolygon(thematic.PolygonBrush4, points);
                        break;
                    case 5:
                        graphics.FillPolygon(thematic.PolygonBrush5, points);
                        break;
                    default:
                        graphics.FillPolygon(thematic.PolygonBrush, points);
                        break;


                }
            }
           
            else
            {
                graphics.FillPolygon(thematic.PolygonBrush, points);
            }
            graphics.DrawPolygon(thematic.PolygonPen, points);
        }
    }

}
