function Send-Greeting {
    # Copyright (c) 2025 Anthony J. Raymond, MIT License
    [CmdletBinding()]
    [OutputType([string])]
    param (
        [Parameter(
            Mandatory,
            ValueFromPipeline
        )]
        [AllowEmptyString()]
        [string[]] $InputObject
    )

    ## LOGIC ###################################################################
    begin {
        [string] $Greeting = Get_Greeting
    }

    process {
        foreach ($Object in $InputObject) {
            if ([string]::IsNullOrEmpty($Object)) {
                $PSCmdlet.WriteError(( New_GreetingArgumentException -Message "Cannot bind argument to parameter 'InputObject' because it is an empty string." ))

                continue
            }

            $PSCmdlet.WriteObject("${Greeting}, ${Object}.")
        }
    }
}
