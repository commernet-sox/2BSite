using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WX_Site.Common
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 获取list中随机的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramList"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> getRandomList<T>(List<T> paramList, int count)
        {
            if (paramList.Count() < count)
            {
                return paramList;
            }
            Random random = new Random();
            List<int> tempList = new List<int>();
            List<T> newList = new List<T>();
            int temp = 0;
            for (int i = 0; i < count; i++)
            {
                temp = random.Next(paramList.Count());//将产生的随机数作为被抽list的索引
                if (!tempList.Contains(temp))
                {
                    tempList.Add(temp);
                    newList.Add(paramList.ElementAt(temp));
                }
                else
                {
                    i--;
                }
            }
            return newList;
        }
    }
}
