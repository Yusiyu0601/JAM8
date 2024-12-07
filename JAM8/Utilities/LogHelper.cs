namespace JAM8.Utilities
{
    /// <summary>
    /// 日志辅助类(EZLogger)
    /// </summary>
    public class LogHelper
    {
        #region Attributes

        /// <summary>
        /// Log levels.
        /// </summary>
        public enum Level
        {
            /// <summary>Log debug messages.</summary>
            Debug = 1,

            /// <summary>Log informational messages.</summary>
            Info = 2,

            /// <summary>Log success messages.</summary>
            Success = 4,

            /// <summary>Log warning messages.</summary>
            Warning = 8,

            /// <summary>Log error messages.</summary>
            Error = 16,

            /// <summary>Log fatal errors.</summary>
            Fatal = 32,

            /// <summary>Log all messages.</summary>
            All = 0xFFFF,
        }

        /// <summary>
        /// The logger's state.
        /// </summary>
        public enum State
        {
            /// <summary>The logger is stopped.</summary>
            Stopped = 0,

            /// <summary>The logger has been started.</summary>
            Running,

            /// <summary>The logger is paused.</summary>
            Paused,
        }

        #endregion

        #region Construction/destruction

        /// <summary>
        /// Constructs a EZLogger.
        /// </summary>
        /// <param name="logFilename">Log file to receive output.</param>
        /// <param name="bAppend">Flag: append to existing file (if any).</param>
        /// <param name="logLevels">Mask indicating log levels of interest.</param>
        public LogHelper
          (string logFilename,
           bool bAppend,
           uint logLevels)
        {
            _logFilename = logFilename;
            _bAppend = bAppend;
            _levels = logLevels;
        }

        /// <summary>
        /// Private default constructor.
        /// </summary>
        private LogHelper()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the log level.
        /// </summary>
        public uint Levels
        {
            get
            {
                return _levels;
            }
            set
            {
                _levels = value;
            }
        }

        /// <summary>
        /// Retrieves the logger's state.
        /// </summary>
        public State LoggerState
        {
            get
            {
                return _state;
            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Starts logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Start()
        {
            lock (this)
            {
                // Fail if logging has already been started
                if (LoggerState != State.Stopped)
                    return false;

                // Fail if the log file isn't specified
                if (String.IsNullOrEmpty(_logFilename))
                    return false;

                // Delete log file if it exists
                if (!_bAppend)
                {
                    try
                    {
                        File.Delete(_logFilename);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                // Open file for writing - return on error
                if (!File.Exists(_logFilename))
                {
                    try
                    {
                        _logFile = File.CreateText(_logFilename);
                    }
                    catch (Exception)
                    {
                        _logFile = null;
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        _logFile = File.AppendText(_logFilename);
                    }
                    catch (Exception)
                    {
                        _logFile = null;
                        return false;
                    }
                }
                _logFile.AutoFlush = true;

                // Return successfully
                _state = LogHelper.State.Running;
                return true;
            }
        }

        /// <summary>
        /// Temporarily suspends logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Pause()
        {
            lock (this)
            {
                // Fail if logging hasn't been started
                if (LoggerState != State.Running)
                    return false;

                // Pause the logger
                _state = LogHelper.State.Paused;
                return true;
            }
        }

        /// <summary>
        /// Resumes logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Resume()
        {
            lock (this)
            {
                // Fail if logging hasn't been paused
                if (LoggerState != State.Paused)
                    return false;

                // Resume logging
                _state = LogHelper.State.Running;
                return true;
            }
        }

        /// <summary>
        /// Stops logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Stop()
        {
            lock (this)
            {
                // Fail if logging hasn't been started
                if (LoggerState != State.Running)
                    return false;

                // Stop logging
                try
                {
                    _logFile.Close();
                    _logFile = null;
                }
                catch (Exception)
                {
                    return false;
                }
                _state = LogHelper.State.Stopped;
                return true;
            }
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Debug
          (string msg)
        {
            _debugMsgs++;
            return WriteLogMsg(Level.Debug, msg);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Info
          (string msg)
        {
            _infoMsgs++;
            return WriteLogMsg(Level.Info, msg);
        }

        /// <summary>
        /// Logs a success message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Success
          (string msg)
        {
            _successMsgs++;
            return WriteLogMsg(Level.Success, msg);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Warning
          (string msg)
        {
            _warningMsgs++;
            return WriteLogMsg(Level.Warning, msg);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Error
          (string msg)
        {
            _errorMsgs++;
            return WriteLogMsg(Level.Error, msg);
        }

        /// <summary>
        /// Logs a fatal error message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Fatal
          (string msg)
        {
            _fatalMsgs++;
            return WriteLogMsg(Level.Fatal, msg);
        }

        /// <summary>
        /// Retrieves the count of messages logged at one or more levels.
        /// </summary>
        /// <param name="levelMask">Mask indicating levels of interest.</param>
        /// <returns></returns>
        public uint GetMessageCount
          (uint levelMask)
        {
            uint uMessages = 0;
            if ((levelMask & ((uint)Level.Debug)) != 0)
                uMessages += _debugMsgs;
            if ((levelMask & ((uint)Level.Info)) != 0)
                uMessages += _infoMsgs;
            if ((levelMask & ((uint)Level.Success)) != 0)
                uMessages += _successMsgs;
            if ((levelMask & ((uint)Level.Warning)) != 0)
                uMessages += _warningMsgs;
            if ((levelMask & ((uint)Level.Error)) != 0)
                uMessages += _errorMsgs;
            if ((levelMask & ((uint)Level.Fatal)) != 0)
                uMessages += _fatalMsgs;
            return uMessages;
        }


        #endregion

        #region Helper methods

        /// <summary>
        /// Writes a log message.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool WriteLogMsg
          (Level level,
           string msg)
        {
            lock (this)
            {

                // Fail if logger hasn't been started
                if (LoggerState == State.Stopped)
                    return false;

                // Ignore message logging is paused or it doesn't pass the filter
                if ((LoggerState == State.Paused) || ((_levels & (uint)level) != (uint)level))
                    return true;

                // Write log message
                DateTime tmNow = DateTime.Now;
                string logMsg = String.Format("{0} {1}  {2}: {3}",
                                               tmNow.ToShortDateString(), tmNow.ToLongTimeString(),
                                               level.ToString().Substring(0, 1), msg);
                _logFile.WriteLine(logMsg);
                return true;
            }
        }

        #endregion

        #region Fields

        /// <summary>Name of the log file.</summary>
        private string _logFilename;

        /// <summary>Flag: append to existing file (if any).</summary>
        private bool _bAppend = true;

        /// <summary>The log file.</summary>
        private StreamWriter _logFile = null;

        /// <summary>Levels to be logged.</summary>
        private uint _levels = (uint)(Level.Warning | Level.Error | Level.Fatal);

        /// <summary>The logger's state.</summary>
        private State _state = State.Stopped;

        /// <summary>Number of debug messages that have been logged.</summary>
        private uint _debugMsgs = 0;

        /// <summary>Number of informational messages that have been logged.</summary>
        private uint _infoMsgs = 0;

        /// <summary>Number of success messages that have been logged.</summary>
        private uint _successMsgs = 0;

        /// <summary>Number of warning messages that have been logged.</summary>
        private uint _warningMsgs = 0;

        /// <summary>Number of error messages that have been logged.</summary>
        private uint _errorMsgs = 0;

        /// <summary>Number of fatal messages that have been logged.</summary>
        private uint _fatalMsgs = 0;

        #endregion
    }

    /* ----------------------- LogHelper 使用说明 -----------------------

    创建和启动 logger
        uint logLevels = (uint) LogHelper.Level.All;
        LogHelper logger =
        new LogHelper ("C:\\EZLoggerTester.log",  // log filename
                      false,                     // don't append
                      logLevels);                // log levels of interest
        bool bStatus = logger.Start();

    输出日志信息
        logger.Info ("An informational message");
        logger.Warning ("A warning");
        logger.Fatal ("A fatal error has occured");
        ...
        // Stop logging
        logger.Stop();

    日志信息的格式
        //12/4/2006 4:38:26 PM  I: An informational message
        //12/4/2006 4:38:26 PM  W: A warning
        //12/4/2006 4:38:26 PM  F: A fatal error has occur

    暂停和重启 
        //Starting long, boring operation
        logger.Pause();
        performLongBoringOperation();
        logger.Resume();

    设置日志级别
        // Only interested in errors and debug msgs
        logger.Levels = (uint) (EZLogger.Level.Error | EZLogger.Level.Debug);
        logger.Info ("...");   // won't be logged
        logger.Error ("...");  // will be logged
*/
}
