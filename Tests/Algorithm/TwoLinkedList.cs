using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Algorithm
{
    /// <summary>
    /// 给出两个 非空 的链表用来表示两个非负的整数。其中，它们各自的位数是按照 逆序 的方式存储的，并且它们的每个节点只能存储 一位 数字。
    /// 如果，我们将这两个数相加起来，则会返回一个新的链表来表示它们的和。
    /// 您可以假设除了数字 0 之外，这两个数都不会以 0 开头。
    /// 示例：
    /// 输入：(2 -> 4 -> 3) + (5 -> 6 -> 4)
    /// 输出：7 -> 0 -> 8
    /// 原因：342 + 465 = 807
    /// 来源：力扣（LeetCode）
    /// 链接：https://leetcode-cn.com/problems/add-two-numbers
    /// 著作权归领扣网络所有。商业转载请联系官方授权，非商业转载请注明出处。
    /// </summary>
    public class TwoLinkedList
    {
        [Fact]
        public void Test()
        {
            var sum = AddTwoNumbers(null, null);
            sum.ShouldNotBeNull();
        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            return GetNode(l1, l2, 0);
        }

        private ListNode GetNode(ListNode l1, ListNode l2, int? add)
        {
            if (l1 == null && l2 == null && !add.HasValue)
            {
                return null;
            }

            ListNode current;
            var valueC = add.HasValue ? add.GetValueOrDefault() : 0;
            var valueA = l1 != null ? l1.val : 0;
            var valueB = l2 != null ? l2.val : 0;
            var sum = valueA + valueB + valueC;
            var multiple = sum / 10;
            var remainder = sum % 10;
            current = new ListNode(remainder);

            if (l1?.next != null || l2?.next != null || multiple > 0)
            {
                if (multiple > 0)
                {
                    current.next = GetNode(l1?.next, l2?.next, 1);
                }
                else
                {
                    current.next = GetNode(l1?.next, l2?.next, null);
                }
            }

            return current;
        }
    }

    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }
}
