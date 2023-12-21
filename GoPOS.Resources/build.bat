
XCOPY %1\config\** %2\config /i /d /y /s /e
XCOPY %1\libs\** %2\libs /i /d /y /s /e
XCOPY %1\data\** %2\data /i /d /y /s /e /exclude:excludeexts.txt