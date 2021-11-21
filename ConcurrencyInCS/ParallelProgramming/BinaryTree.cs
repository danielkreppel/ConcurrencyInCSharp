using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyInCS.ParallelProgramming
{
    public class BinaryTree
    {
        public int Value;
        public BinaryTree Left;
        public BinaryTree Right;

        public BinaryTree(int[] values) : this(values, 0) { }

        BinaryTree(int[] values, int index)
        {
            Load(this, values, index);
        }

        void Load(BinaryTree tree, int[] values, int index)
        {
            this.Value = values[index];
            if (index * 2 + 1 < values.Length)
            {
                this.Left = new BinaryTree(values, index * 2 + 1);
            }
            if (index * 2 + 2 < values.Length)
            {
                this.Right = new BinaryTree(values, index * 2 + 2);
            }
        }
    }
}
