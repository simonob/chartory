chartory
========

XAML/C# Windows 8 UI (Metro) -style charts

Hello. This project started a while back, prior to Telerik releasing their first set of Metro-style charting components, due to a need for charting components within a Windows 8 UI (Metro)-style application I was developing.

However, while I suspect there will be a number of commercially available charting components for Windows 8 Metro, I still feel there is some value in developing an open-source alternative suite of charting (and perhaps wider data visualisation) controls, and hence the decision to create the project and see what happens!

I've also been a consumer of open-source projects for a number of years, and never given anything back to the community; so hopefully this is my opportunity.

## What's in it?

Currently a basic pie chart, featuring
* Data binding, INotifyPropertyChanged and INotifyCollectionChanged support
* On click/tap events and explosion of pie pieces
* Colour palette override
* Variable sized "Hole in the middle" / forced radius
* Templatable legend control

The source currently has a test application with two pie charts which should be a good starting point!

## Future aims

* Documentation
* Bar charts
* Line graphs
* Data viz
* Improved animation (nice transition in would be good)
* NuGet package
* Improved design?
* Performance improvements
* Async?

Finally, at the moment the chart rebuilds each of its pie segments on each data change... I suspect this will work for smaller data sets, but not scale very well. So final goal would be to get the pie pieces to change, rather than rebuild, along with some animation.

## Feedback / issues

If you find an issue, please [log it](https://github.com/simonob/chartory/issues/new) and I'll take a look. Alternatively feel free to fork and take a look yourself!

I'd also welcome feedback on the design, good or bad! If bad and it warrants raising an issue, do that, otherwise you can get hold of me via twitter ([@SimonOBeirne](http://twitter.com/SimonOBeirne))

Finally, if you'd like to help me out in my quest, please give a shout!
