function New_GreetingArgumentException {
    # Copyright (c) 2025 Anthony J. Raymond, MIT License

    [CmdletBinding()]
    [OutputType([System.Management.Automation.ErrorRecord])]
    param(
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $Message,

        [Parameter()]
        [switch] $Throw = $false
    )

    ## LOGIC ###################################################################
    end {
        [System.Management.Automation.ErrorRecord] $ErrorRecord = [System.Management.Automation.ErrorRecord]::new(
            [PoshGUIExample.GreetingArgumentException] $Message,
            'PoshGUIExample.GreetingArgumentException',
            [System.Management.Automation.ErrorCategory]::InvalidArgument,
            $null
        )

        if ($Throw) {
            throw $ErrorRecord
        }

        $PSCmdlet.WriteObject($ErrorRecord)
    }
}
