@echo off
cd Resources/Prototypes
python ../../Tools/ss14_ru/yamlextractor.py
pause
python ../../Tools/ss14_ru/keyfinder.py
pause
