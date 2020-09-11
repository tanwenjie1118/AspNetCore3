﻿using IdentityServer4.Validation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var flag = CanWin(new int[] { 1, 5, 2 });
            //var flag = CanWin(new int[] { 2, 4, 55, 6, 8 });
            flag.ShouldBeTrue();
        }

        private bool CanWin(int[] nums)
        {
            int A = 0;
            int B = 0;

            var f = Func(nums.ToList(), A, B);
            return f;
        }

        private bool Func(List<int> list, int A, int B)
        {
            var copy = new int[list.Count];
            list.CopyTo(copy);
            var list1 = copy.ToList();
            // 从前取第一个数
            if (AGetFrontSide(list, A, B))
            {
                return true;
            }

            // 从后取第一个数
            if (AGetEndSide(list1, A, B))
            {
                return true;
            }

            return false;
        }

        private bool AGetFrontSide(List<int> list, int A, int B)
        {
            // 玩家一需要通过取值 找到一个方法永远可以赢玩家二
            var first = list.FirstOrDefault();
            A += first;
            StackPop(list, true);

            if (list.Count == 0)
            {
                if (A >= B)
                {
                    Debug.WriteLine($"当前 A =={A} B == {B}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (list.Count == 1)
            {
                B += list.FirstOrDefault();
                StackPop(list, false);
                if (A >= B)
                {
                    Debug.WriteLine($"当前 A =={A} B == {B}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var bfirst = list.FirstOrDefault();
                var blast = list.LastOrDefault();

                // 分两种情况 取前值 取后值
                var cache = new int[list.Count];
                var cache1 = new int[list.Count];
                list.CopyTo(cache);
                list.CopyTo(cache1);
                var B1 = B;
                var B2 = B;
                var _list = cache.ToList();
                var _list1 = cache1.ToList();

                // 取前值
                StackPop(_list, true);
                B1 += bfirst;

                // 取后值
                StackPop(_list1, false);
                B2 += blast;

                if (Func(_list1, A, B2) && Func(_list, A, B1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool AGetEndSide(List<int> list, int A, int B)
        {
            var last = list.LastOrDefault();
            A += last;
            StackPop(list, false);

            if (list.Count == 0)
            {
                if (A >= B)
                {
                    Debug.WriteLine($"当前 A =={A} B == {B}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (list.Count == 1)
            {
                B += list.FirstOrDefault();
                StackPop(list, false);
                if (A >= B)
                {
                    Debug.WriteLine($"当前 A =={A} B == {B}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var bfirst = list.FirstOrDefault();
                var blast = list.LastOrDefault();

                // 分两种情况 取前值 取后值
                var cache = new int[list.Count];
                var cache1 = new int[list.Count];
                list.CopyTo(cache);
                list.CopyTo(cache1);
                var B1 = B;
                var B2 = B;
                var _list = cache.ToList();
                var _list1 = cache1.ToList();

                // 取前值
                StackPop(_list, true);
                B1 += bfirst;

                // 取后值
                StackPop(_list1, false);
                B2 += blast;
                if (Func(_list1, A, B2) && Func(_list, A, B1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void StackPop(List<int> list, bool ishead)
        {
            if (ishead)
            {
                Debug.WriteLine("删除头部：" + list.FirstOrDefault());
                list.RemoveAt(0);

            }
            else
            {
                Debug.WriteLine("删除尾部：" + list.LastOrDefault());
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
