function Get_Greeting {
    # Copyright (c) 2025 Anthony J. Raymond, MIT License
    [CmdletBinding()]
    [OutputType([string])]
    param ()

    ## LOGIC ###################################################################
    end {
        switch ((Get-Date).Hour) {
            { $_ -ge 18 } { return 'Good Evening' }
            { $_ -ge 12 } { return 'Good Afternoon' }
            { $_ -ge 6 } { return 'Good Morning' }
            { $_ -ge 0 } { return 'Good Night' }
        }
    }
}
