using System;
using System.Collections.Generic;

namespace UnityUtils.Extensions
{
    public static class QueueExtensions
    {
        public static Queue<T> Shuffle<T>(this Queue<T> queue)
        {
            Random rng = new Random();

            // Diziyi listeye kopyala
            T[] array = queue.ToArray();

            // Karıştır
            int arrayLength = array.Length;
            while (arrayLength > 1)
            {
                arrayLength--;
                int index = rng.Next(arrayLength + 1);
                
                //İtemleri yer değiş
                (array[index], array[arrayLength]) = (array[arrayLength], array[index]);
            }

            // Karıştırılmış elemanları yeni bir kuyruğa ekle
            Queue<T> shuffledQueue = new Queue<T>(array);

            return shuffledQueue;
        }
    }
}