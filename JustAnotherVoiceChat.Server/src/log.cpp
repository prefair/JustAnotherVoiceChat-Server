/*
 * File: src/log.cpp
 * Date: 17.02.2018
 *
 * MIT License
 *
 * Copyright (c) 2018 JustAnotherVoiceChat
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#include "log.h"

static logMessageCallback_t _logMessageCallback = nullptr;

int _logLevel = LOG_LEVEL_INFO;

void setLogLevel(int logLevel) {
    _logLevel = logLevel;

    if (_logLevel > LOG_LEVEL_TRACE) {
        _logLevel = LOG_LEVEL_TRACE;
    } else if (_logLevel < LOG_LEVEL_ERROR) {
        _logLevel = LOG_LEVEL_ERROR;
    }
}

void logMessage(std::string message, int level) {
    if (_logMessageCallback == nullptr) {
        return;
    }

    if (level > _logLevel) {
        return;
    }

    _logMessageCallback(message.c_str(), level);
}

void setLogMessageCallback(logMessageCallback_t callback) {
    _logMessageCallback = callback;
}
