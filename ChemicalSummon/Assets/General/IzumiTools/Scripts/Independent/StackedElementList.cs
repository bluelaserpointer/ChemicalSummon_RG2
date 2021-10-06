using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class StackedElementList<T> : IEnumerable<StackedElementList<T>.StackedElement>, IEnumerable
{
    public StackedElementList() { }
    public StackedElementList(StackedElementList<T> sample)
    {
        foreach (var stackedElement in sample.list)
            list.Add(new StackedElement(stackedElement));
    }
    [Serializable]
    public class StackedElement
    {
        public T type;
        public int amount;
        public StackedElement(T type, int amount = 1)
        {
            this.type = type;
            this.amount = amount;
        }
        public StackedElement(StackedElement sample)
        {
            type = sample.type;
            amount = sample.amount;
        }
    }
    public List<StackedElement> list = new List<StackedElement>();
    public List<T> Types
    {
        get
        {
            List<T> types = new List<T>();
            foreach(var each in list)
            {
                types.Add(each.type);
            }
            return types;
        }
    }
    public enum AmountAcception { POSITIVE, NOT_NEGATIVE, ALL}
    public AmountAcception amountAcception = AmountAcception.POSITIVE;
    public void SetAmountAccept(AmountAcception acception)
    {
        amountAcception = acception;
    }
    protected bool AcceptAmount(int amount)
    {
        return amount > 0 || amountAcception.Equals(AmountAcception.NOT_NEGATIVE) && amount == 0 || amountAcception.Equals(AmountAcception.ALL);
    }
    public StackedElement FindByType(T type)
    {
        return list.Find(each => each.type.Equals(type));
    }
    public int CountStack(T type)
    {
        StackedElement find = FindByType(type);
        return find != null ? find.amount : 0;
    }
    public int CountStack()
    {
        int total = 0;
        list.ForEach(each => total += each.amount);
        return total;
    }
    public int CountType()
    {
        return list.Count;
    }
    public bool IsEmpty => CountType() == 0;
    public bool Set(T type, int amount)
    {
        StackedElement find = FindByType(type);
        if (find != null)
        {
            if (!AcceptAmount(amount))
                list.Remove(find);
            return true;
        }
        else if (AcceptAmount(amount))
        {
            list.Add(new StackedElement(type, amount));
            return true;
        }
        return false;
    }
    public bool Add(T type, int amount = 1)
    {
        StackedElement find = FindByType(type);
        if (find != null)
        {
            if (!AcceptAmount(find.amount += amount))
                list.Remove(find);
            return true;
        }
        else if (AcceptAmount(amount))
        {
            list.Add(new StackedElement(type, amount));
            return true;
        }
        return false;
    }
    public bool Add(StackedElement stackedElement)
    {
        return Add(stackedElement.type, stackedElement.amount);
    }
    public void AddAll(StackedElementList<T> anotherList)
    {
        anotherList.ForEach(each => Add(each.type, each.amount));
    }
    public bool Remove(T type, int amount = 1)
    {
        return Add(type, -amount);
    }
    public bool Remove(StackedElement stackedElement)
    {
        return Remove(stackedElement.type, stackedElement.amount);
    }
    public bool RemoveAll(StackedElementList<T> anotherList)
    {
        bool cond = true;
        foreach(var stack in anotherList)
        {
            if (!Remove(stack))
                cond = false;
        }
        return cond;
    }
    public void ForEach(Action<StackedElement> action)
    {
        list.ForEach(each => action.Invoke(each));
    }
    public IEnumerator<StackedElement> GetEnumerator()
    {
        return list.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }
    public void Clear()
    {
        list.Clear();
    }
    public void SortByStackAmount(bool increasing)
    {
        if(increasing)
            list.Sort((stack1, stack2) => stack1.amount - stack2.amount);
        else
            list.Sort((stack1, stack2) => stack2.amount - stack1.amount);
    }
    public bool ContentsEquals(StackedElementList<T> anotherStackList)
    {
        if (CountType() != anotherStackList.CountType())
            return false;
        foreach (var stack in list)
        {
            if (anotherStackList.CountStack(stack.type) != stack.amount)
                return false;
        }
        return true;
    }
}