using IdentityServer4.Validation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests.Algorithm
{
    /// <summary>
    /// 给定一个表示分数的非负整数数组。 玩家 1 从数组任意一端拿取一个分数，随后玩家 2 继续从剩余数组任意一端拿取分数，然后玩家 1 拿，…… 。每次一个玩家只能拿取一个分数，分数被拿取之后不再可取。直到没有剩余分数可取时游戏结束。最终获得分数总和最多的玩家获胜。
    ///给定一个表示分数的数组，预测玩家1是否会成为赢家。你可以假设每个玩家的玩法都会使他的分数最大化。
    ///from：https://leetcode-cn.com/problems/predict-the-winner
    /// </summary>
    public class Winner
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void Test2()
        {
            var flag = CanWin(new int[] {2, 4, 55, 6, 8});
            flag.ShouldBeTrue();
        }

        private bool CanWin(int[] nums)
        {
            int A = 0;
            int B = 0;

            var f1 = Func(nums.ToList(), A, B);
            return f1;
        }

        private bool Func(List<int> list, int A, int B)
        {
            var copy = new int[list.Count];
            list.CopyTo(copy);
            var list1 = copy.ToList();
            int A1 = A;
            int B1 = B;

            #region 取前端值

            // 玩家一需要通过取值 找到一个方法永远可以赢玩家二
            var first = list.FirstOrDefault();
            A += first;
            list.RemoveAt(0);

            if (list.Count == 0)
            {
                if (A >= B)
                {
                    return true;
                }
            }
            else
            {
                // 玩家二永远取最大值
                var bfirst = list.FirstOrDefault();
                var blast = list.LastOrDefault();
                if (blast > bfirst)
                {
                    B += blast;
                    list.RemoveAt(list.Count - 1);
                }
                else
                {
                    B += bfirst;
                    list.RemoveAt(0);
                }
            }

            #endregion

            #region 取后端值
            var last = list1.LastOrDefault();
            A1 += last;
            list1.RemoveAt(list1.Count - 1);

            if (list1.Count == 0)
            {
                if (A1 >= B1)
                {
                    return true;
                }
            }
            else
            {
                // 玩家二永远取最大值
                var bfirst1 = list1.FirstOrDefault();
                var blast1 = list1.LastOrDefault();
                if (bfirst1 > blast1)
                {
                    B1 += bfirst1;
                    list1.RemoveAt(0);
                }
                else
                {
                    B1 += blast1;
                    list1.RemoveAt(list1.Count - 1);
                }
            }

            #endregion

            if (list1.Count == 0)
            {
                return A1 >= B1 || A >= B;
            }
            else
            {
                return Func(list, A, B) || Func(list1, A1, B1);
            }
        }
    }
}
