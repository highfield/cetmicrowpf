using System;
using Microsoft.SPOT;
using System.Collections;

namespace Toolbox.NETMF.MicroWPF
{
    public delegate Page PageCreationDelegate();


    public class NavigationService
    {

        private ArrayList _pageStack = new ArrayList();
        private int _currentIndex = -1;
        private int _nextIndex = -1;
        private Page _nextPage;


        public Page CurrentPage
        {
            get
            {
                return this._currentIndex >= 0
                    ? (Page)this._pageStack[this._currentIndex]
                    : (Page)null;
            }
        }


        public void Navigate(Page page)
        {
            this._nextPage = page;
            this._nextIndex = this._currentIndex + 1;
        }


        public bool CanGoBack
        {
            get { return this._currentIndex > 0; }
        }


        public void GoBack()
        {
            if (this._currentIndex > 0)
            {
                this._nextIndex = this._currentIndex - 1;
                this._nextPage = null;
            }
        }


        public void Clock(FT800Device dc)
        {
            if (this._nextIndex != this._currentIndex)
            {
                bool proceed = true;
                if (this._currentIndex >= 0)
                {
                    var args = new NavigationLeaveArgs();
                    args.GoingBack = this._nextIndex < this._currentIndex;
                    this.CurrentPage.InvokeBeforeLeave(dc, args);
                    if (args.Cancel) proceed = false;
                }

                if (proceed)
                {
                    if (this._nextIndex > this._currentIndex)
                    {
                        if (this._nextPage != null)
                        {
                            for (int i = this._pageStack.Count - 1; i > this._currentIndex; i--)
                            {
                                this._pageStack.RemoveAt(i);
                            }

                            this._pageStack.Add(this._nextPage);
                            this._currentIndex++;

                            this._nextPage.InvokeCreate(dc);
                        }
                        else if (this._nextIndex < this._pageStack.Count)
                        {
                            this._currentIndex = this._nextIndex;
                        }
                    }
                    else
                    {
                        this._currentIndex = this._nextIndex;
                    }

                    this.CurrentPage.InvokeEnter(dc);
                    this.CurrentPage.Invalidate(true);
                    this._nextPage = null;
                }
            }
            else if (this._currentIndex < 0)
            {
                return;
            }

            this.CurrentPage.Render(dc);
        }


        public NavigationService GetNavigationService(WidgetBase source)
        {
            //TODO
        }

    }


    public class NavigationLeaveArgs
    {
        public bool GoingBack { get; internal set; }
        public bool Cancel;
    }
}
