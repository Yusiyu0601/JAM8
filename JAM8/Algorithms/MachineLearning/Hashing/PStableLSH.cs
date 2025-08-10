using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.MachineLearning
{
    /// <summary>
    /// 名称：PStableLSH
    /// 作者：喻思羽
    /// 时间：2016-5-3
    /// 说明：p-stable LSH 算法
    /// </summary>
    public class PStableLSH
    {
        private int seed;//随机种子
        private int HashTableCount = 3;//哈希表的数量
        private int Dimension = 2;//Vector的维度
        private float width;//LSH的w
        private float b;//LSH的随机数b
        private float[][] a = null;//p-Stable分布（L=2；高斯分布）的随机向量

        /// <summary>
        /// 哈希表
        /// </summary>
        private List<HashTable> HashTables { get; set; }

        /// <summary>
        /// 输入的Vectors
        /// </summary>
        public List<MyVector> Vectors { get; internal set; }

        /// <summary>
        /// PStableLSH 构造函数
        /// 
        /// 
        /// </summary>
        /// <param name="HashTableCount">哈希表的数量</param>
        /// <param name="Dimension">输入数据的维度</param>
        /// <param name="Width">LSH的w（带宽）</param>
        /// <param name="b">LSH的随机数b</param>
        public PStableLSH(int HashTableCount, int Dimension, float width, float b, int seed)
        {
            this.HashTableCount = HashTableCount;
            this.Dimension = Dimension;
            this.width = width;
            this.b = b;
            this.seed = seed;
        }

        //初始化LSH参数
        private void InitLSH(int seed)
        {
            MersenneTwister mt = new((uint)seed);
            //p-Stable分布（L=2；高斯分布）的随机向量
            a = new float[HashTableCount][];
            Gaussian gaussian = new(0, 1);
            //生成随机的哈希向量
            for (int j = 0; j < HashTableCount; j++)
            {
                float[] a1 = new float[Dimension];
                for (int k = 0; k < Dimension; k++)
                {
                    a1[k] = (float)gaussian.Sample(mt);
                }
                a[j] = a1;
            }

            HashTables = new List<HashTable>();//产生指定数量的哈希表
            for (int l = 0; l < HashTableCount; l++)
            {
                HashTables.Add(new HashTable());
            }
        }

        //哈希函数，f为特征，a_temp为a向量，b_temp为b，w_temp为w
        private int Hashfamily(float[] f, float[] a_temp, float b_temp, float w_temp)
        {
            int dim = f.Length;
            float result = b_temp;
            for (int i = 0; i < dim; i++)
            {
                result += f[i] * a_temp[i];
            }
            return (int)(result / w_temp);//返回哈希结果
        }

        /// <summary>
        /// 将Vector映射到哈希表，每次映射计算结果覆盖前一次结果
        /// </summary>
        /// <param name="Vectors"></param>
        public void MapVectorToHashTable(List<MyVector> Vectors)
        {
            InitLSH(seed);
            this.Vectors = Vectors;
            for (int i = 0; i < Vectors.Count; i++)//计算vector在每个哈希表里的哈希值
            {
                //哈希过程
                for (int l = 0; l < HashTableCount; l++)
                {
                    //逐个哈希函数计算点的对应哈希key
                    int hash_num = Hashfamily(Vectors[i].buffer, a[l], b, width);
                    int key = (int)(hash_num / width);
                    //保存哈希key
                    HashTables[l].Add(key, i);
                }
            }
        }

        /// <summary>
        /// 查询vector的相似最邻近对象
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public (List<int>, List<MyVector>) Search(MyVector vector)
        {
            List<int> NearestIndexes = new();
            List<MyVector> Nearest = new();
            //哈希过程
            for (int l = 0; l < HashTableCount; l++)//逐个哈希函数计算点的对应哈希key
            {
                int hash_num = Hashfamily(vector.buffer, a[l], b, width);//哈希
                int key = (int)(hash_num / width);//哈希表的key
                var VectorIndexes = HashTables[l].GetVectorIndexes(key);//哈希存储
                if (VectorIndexes != null)
                    NearestIndexes.AddRange(VectorIndexes);
            }
            //Indexes去重复
            NearestIndexes = NearestIndexes.Distinct().ToList();
            foreach (var i in NearestIndexes)
            {
                Nearest.Add(Vectors[i]);
            }
            return (NearestIndexes, Nearest);
        }
    }

    /// <summary>
    /// 名称：HashTable 哈希表
    /// 作者：喻思羽
    /// 时间：2016-5-1
    /// 
    /// 说明：基于p-stable hashing的相似最近邻查询
    /// </summary>
    internal class HashTable
    {
        //key:哈希码 value:（哈希值相同的）矢量索引的集合
        internal Dictionary<int, List<int>> data
        {
            get; set;
        }

        /// <summary>
        /// HashTable 构造函数
        /// </summary>
        internal HashTable()
        {
            data = new Dictionary<int, List<int>>();
        }

        private bool ContainsKey(int Key)
        {
            return data.ContainsKey(Key);
        }

        internal void Add(int Key, int VectorIndex)
        {
            if (ContainsKey(Key))
            {
                data[Key].Add(VectorIndex);
            }
            else
            {
                data.Add(Key, new List<int>());
                data[Key].Add(VectorIndex);
            }
        }

        internal List<int> GetVectorIndexes(int Key)
        {
            if (ContainsKey(Key))
            {
                return data[Key];
            }
            return null;
        }
    }
}
