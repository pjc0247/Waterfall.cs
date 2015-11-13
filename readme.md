Waterfall.cs
====

__Waterfall__, task continuation pattern.<br>
Made for my integration testing system.
<br>
<br>
Yes. I know that ".Net 4" has VERY NICE DESIGN to handle it, but I'm using Unity3d environment which doesn't support that.

```c#
Waterfall.Begin((Waterfall.Context ctx) =>
{
    var result = SomeTestFunc();

    if (result)
        ctx.Next();
    else
        ctx.Abort("SomeTestFunc => false");
})
.Then((Waterfall.Context ctx) =>
{
    SomeAsyncTestFunc((bool result) => {
      if(result)
        ctx.Next();
      else
        ctx.Abort("SomeAsyncTestFunc => false");
    });
})
.Then((Waterfall.Context ctx) =>
{
    try {
      SomeExceptionTest();

      ctx.Next();
    }
    catch(Exception e) {
      ctx.Abort(e);
    }
})
.Then( /* ... */ )
.Run();
```