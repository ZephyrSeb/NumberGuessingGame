namespace FinalProject {
    static class FPMath {
        /// <summary>
        /// A sorting algorithm to sort the data so it renders in order of best score to worst
        /// </summary>
        /// <param name="dict">The dictionary to be sorted</param>
        /// <returns>A sorted dictionary</returns>
        public static Dictionary<DateTime,int> Sort(Dictionary<DateTime,int> dict) {
            var list = dict.ToList();
            bool sorted = false;
            while (!sorted) {
                sorted = true;
                for (int i = 0; i < list.Count - 1; i++) {
                    if (list.ElementAt(i).Value < list.ElementAt(i + 1).Value) {
                        var temp = list[i];
                        list[i] = list[i+1];
                        list[i+1] = temp;
                        sorted = false;
                    }
                }
            }
            return list.ToDictionary(obj => obj.Key, obj => obj.Value);
        }
    }
}