using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Services.AppMonad;

public static class AppExtensions
{
    public static App<A> As<A>(this K<App, A> kind)
    {
        ArgumentNullException.ThrowIfNull(kind);
        return (App<A>)kind;
    }
    public static App<B> Select<A, B>(this K<App, A> ma, Func<A, B> f)
    {
        ArgumentNullException.ThrowIfNull(ma);
        ArgumentNullException.ThrowIfNull(f);
        return ma.Map(f).As();
    }

    public static App<B> SelectMany<A, B>(
        this K<App, A> ma,
        Func<A, App<B>> f
    )
    {
        ArgumentNullException.ThrowIfNull(ma);
        ArgumentNullException.ThrowIfNull(f);
        return ma.Bind(f).As();
    }

    public static App<B> SelectMany<A, B>(
        this K<App, A> ma,
        Func<A, K<App, B>> f
    )
    {
        ArgumentNullException.ThrowIfNull(ma);
        ArgumentNullException.ThrowIfNull(f);
        return ma.Bind(f).As();
    }

    public static App<C> SelectMany<A, B, C>(
        this K<App, A> ma,
        Func<A, App<B>> f,
        Func<A, B, C> g)
    {
        ArgumentNullException.ThrowIfNull(ma);
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(g);
        return ma.Bind(a => f(a).Map(b => g(a, b))).As();
    }

    public static App<C> SelectMany<A, B, C>(
        this K<App, A> ma,
        Func<A, K<App, B>> f,
        Func<A, B, C> g)
    {
        ArgumentNullException.ThrowIfNull(ma);
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(g);
        return ma.Bind(a => f(a).Map(b => g(a, b))).As();
    }
}
