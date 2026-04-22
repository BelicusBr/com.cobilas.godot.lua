$branchs = git branch

$a_branch

foreach ($branch in $branchs) {
	if ($branch.Contains("*")) {
		$a_branch = $branch.Trim("*").Trim()
	}
}

. git-funcs

Switch -Wildcard ($a_branch) {
	"*.dev" {
		merge "main.fix" $a_branch
		merge "main.dev.gdlua2" "main.fix"
		merge "main" "main.dev.gdlua2"
		git checkout $a_branch
	}
	"*.fix" {
		merge "main.dev" $a_branch
		merge "main.dev.gdlua2" "main.dev"
		merge "main" "main.dev.gdlua2"
		git checkout $a_branch
	}
	"*.dev.gdlua2" {
		merge "main.dev" $a_branch
		merge "main.fix" "main.dev"
		merge "main" "main.fix"
		git checkout $a_branch
	}
	default {
		merge "main.dev" $a_branch
		merge "main.fix" "main.dev"
		merge "main.dev.gdlua2" "main.fix"
		git checkout $a_branch
	}
}

$branchs = git remote

foreach ($branch in $branchs) {
	push-all $branch $True
}