using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Herafi.UWP.Resources.Animations
{
    public class BrushAnimation //: AnimationTimeLine
    {
        //public override Type TargetPropertyType
        //{
        //    get
        //    {
        //        return typeof(Brush);
        //    }
        //}

        //public override object GetCurrentValue(object defaultOriginValue,
        //                                       object defaultDestinationValue,
        //                                       AnimationClock animationClock)
        //{
        //    return GetCurrentValue(defaultOriginValue as Brush,
        //                           defaultDestinationValue as Brush,
        //                           animationClock);
        //}
        //public object GetCurrentValue(Brush defaultOriginValue,
        //                              Brush defaultDestinationValue,
        //                              AnimationClock animationClock)
        //{
        //    if (!animationClock.CurrentProgress.HasValue)
        //        return Brushes.Transparent;

        //    //use the standard values if From and To are not set 
        //    //(it is the value of the given property)
        //    defaultOriginValue = this.From ?? defaultOriginValue;
        //    defaultDestinationValue = this.To ?? defaultDestinationValue;

        //    if (animationClock.CurrentProgress.Value == 0)
        //        return defaultOriginValue;
        //    if (animationClock.CurrentProgress.Value == 1)
        //        return defaultDestinationValue;

        //    return new VisualBrush(new Border()
        //    {
        //        Width = 1,
        //        Height = 1,
        //        Background = defaultOriginValue,
        //        Child = new Border()
        //        {
        //            Background = defaultDestinationValue,
        //            Opacity = animationClock.CurrentProgress.Value,
        //        }
        //    });
        //}

        //protected override Freezable CreateInstanceCore()
        //{
        //    return new BrushAnimation();
        //}

        ////we must define From and To, AnimationTimeline does not have this properties
        //public Brush From
        //{
        //    get { return (Brush)GetValue(FromProperty); }
        //    set { SetValue(FromProperty, value); }
        //}
        //public Brush To
        //{
        //    get { return (Brush)GetValue(ToProperty); }
        //    set { SetValue(ToProperty, value); }
        //}

        //public static readonly DependencyProperty FromProperty =
        //    DependencyProperty.Register("From", typeof(Brush), typeof(BrushAnimation));
        //public static readonly DependencyProperty ToProperty =
        //    DependencyProperty.Register("To", typeof(Brush), typeof(BrushAnimation));
    }
}
