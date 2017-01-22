# Going data driven

Suddenly one evening, I thought that I would try and implement the populary game 2048 in 
.Net Core, using TDD as methodology. As an extension, I would use something like 
[TensorFlow](https://www.tensorflow.org/) to see if it could learn how to play the game.

As it often go, the project took another path, partly because I realized that I might not
get that far in one evening, but also because I found another challange in the task.

In the first version, I implemented the core shift and add algorithms like this:


    public void Right()
    {
        for (int r = 0; r < Size; r++)
        {
            for (int c = Size - 2; c >= 0; c--)
            {
                var current = Get(r, c);
                for (int i = c + 1; i < Size; i++)
                {
                    var candidate = Get(r, i);
                    if (candidate == 0 || candidate == current)
                    {
                        current += candidate;
                        Set(r, i, current);
                        Set(r, i - 1, 0);
                    }
                }
            }
        }
    }

    public void Left()
    {
        for (int r = First; r < Size; r++)
        {
            for (int c = First + 1; c < Size; c++)
            {
                var current = Get(r, c);
                for (int i = c - 1; i >= First; i--)
                {
                    var candidate = Get(r, i);
                    if (candidate == 0 || candidate == current)
                    {
                        current += candidate;
                        Set(r, i, current);
                        Set(r, i + 1, 0);
                    }
                }
            }
        }
    }

When I shift right, I start from the second last column and shift the value to the right.
I then repeat by taking the third last column, moving the value as far right I can.
The algorithm to shift left is very similar.

I realized that I would get two more functions like this, Up, and Down. 
The code duplication in these function made me feel sick. What could I possibly do to improve this?

Well, the first observation was at the innermost level, where I move or add, can be extracted as methd with parameters:


    public void Right()
    {
        for (int ri = First; ri < Size; ri++)
        {
            for (int ci = Last - 1; ci >= First; ci--)
            {
                for (int i = ci + 1; i < Size; i++)
                {
                    Shift(ri, i - 1, i);
                }
            }
        }
    }


    public void Left()
    {
        for (int ri = First; ri < Size; ri++)
        {
            for (int ci = First + 1; ci < Size; ci++)
            {
                for (int i = ci - 1; i >= First; i--)
                {
                    Shift(ri, i + 1, i);
                }
            }
        }
    }

    private void Shift(int r, int from, int to)
    {
        var current = Get(r, from);
        var candidate = Get(r, to);
        if (candidate == 0 || candidate == current)
        {
            current += candidate;
            Set(r, to, current);
            Set(r, from, 0);
        }
    }

Yes! Victory! The algorithm for each direction is now almost readable. Or is it? Maybe we can do better even?
What are we doing here? We loop over *sequences*, maybe we can introduce some Enumerable generator, like this:


    public void Right()
    {
        foreach (int ri in Sequence(First, Last))
            foreach(int ci in Sequence(First, Last - 1).Reverse())
                foreach(int i in Sequence(ci + 1, Last))
                    Shift(ri, i - 1, i);
    }


    private IEnumerable<int> Sequence(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            yield return i;
        }
    }


Well, well, well! That is compact code, and quite readable as well. Next step is of course to use Linq!
Is it possible to generate all candidate shifts as a query?

    public void Right()
    {
        var query = from ri in Sequence(First, Last)
                    from ci in Sequence(First, Last - 1).Reverse()
                    from i in Sequence(ci + 1, Last)
                    select new { rFrom = ri, rTo = ri, cFrom = i - 1, cTo = i };

        foreach (var item in query)
        {
            Shift(item.rFrom, item.rTo, item.cFrom, item.cTo);
        }                    
    }

Yup, that works. Lets clean up that a bit, and add a method that takes all the candidates to shift.

    public void Right()
    {
        ShiftAll(from ri in Sequence(First, Last)
                 from ci in Sequence(First, Last - 1).Reverse()
                 from i in Sequence(ci + 1, Last)
                 select new Index(ri, ri, i - 1, i));

    }

    public void Left()
    {
        ShiftAll(from ri in Sequence(First, Last)
                 from ci in Sequence(First + 1, Last)
                 from i in Sequence(First, ci - 1).Reverse()
                 select new Index(ri, ri, i + 1, i));
    }

    private void ShiftAll(IEnumerable<Index> indices)
    {
        foreach (var item in indices) Shift(item);
    }

    private void Shift(Index id)
    {
        var current = Get(id.rFrom, id.cFrom);
        var candidate = Get(id.rTo, id.cTo);
        if (candidate == 0 || candidate == current)
        {
            current += candidate;
            Set(id.rTo, id.cTo, current);
            Set(id.rFrom, id.cFrom, 0);
        }
    }

    private IEnumerable<int> Sequence(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            yield return i;
        }
    }

That is almost spooky! With some more magic, I think we can get rid of the for loops completely!

We went from an extremely old-school, procedural code to a more data-driven and declarative algorithm.
The Up and Down methods will look very much the same, but will shift row and column. 

It is bed time!


