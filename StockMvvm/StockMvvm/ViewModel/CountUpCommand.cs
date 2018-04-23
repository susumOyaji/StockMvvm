using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace StockMvvm
{
    internal class CountUpCommand : ICommand
    {
        #region メンバ変数

        private readonly Action _action;
        //private Action<Price>  _priceaction;

        #endregion

        #region イベント

        /// <summary>
        /// 概要:
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生します。
        /// </summary>
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            var h = this.CanExecuteChanged;
            if (h != null)
            {
                h(this, EventArgs.Empty);
            }
        }

        #endregion

        #region コンストラクタ

        internal  CountUpCommand(Action action)
        {
            this._action = action;
        }

        //internal CountUpCommand(Action<Price> delAdd)
        //{
        //   this._priceaction = delAdd;
        //}


        #endregion

        #region メソッド

        /// <summary>
        /// ボタンクリック時に呼び出される
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
            //var d = CanExecuteHandler;
            //return d == null ? true : d(parameter);


        }

        /// <summary>
        /// CanExecuteがtrueだったら呼び出される
        /// </summary>
        /// <param name="parameter"></param>
        ///コマンドで実行する処理の実体です。
        ///parameterにはXAML側でCommandParameterというのを指定すると渡されてきます。
        /// CommandParameterがXAML側で指定されていないと、nullが渡ってきます。
        public void Execute(object parameter)
        {
            this._action();
            //this._priceaction();

            //var d = ExecuteHandler;
            //if (d != null)
            //    d(parameter);

        }

        #endregion
    }


    public class DelegateCommand<T> : ICommand
    {

        private readonly Action<Price> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            var h = this.CanExecuteChanged;
            if (h != null)
            {
                h(this, EventArgs.Empty);
            }
        }



        public DelegateCommand(Action<Price> execute) : this(execute, () => true) { }

        public DelegateCommand(Action<Price> execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            this._execute((Price)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }


    }

}